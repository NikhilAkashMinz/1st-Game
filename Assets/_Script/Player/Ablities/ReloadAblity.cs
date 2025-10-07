using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ReloadAblity : BaseAbility
{
    public InputActionReference reloadActionRef;
    [SerializeField] private ReloadBar reloadBar;
    private Weapon currentWeapon;
    private Coroutine reloadCoroutine;

    protected override void Initialization()
    {
        base.Initialization();
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
    }

    public override void EnterAbility()
    {
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
        linkedPhysics.ResetVelocity();
    }
    private void OnEnable()
    {
        reloadActionRef.action.performed += TryToReload;
    }

    private void OnDisable()
    {
        reloadActionRef.action.performed -= TryToReload;
    }

    private void TryToReload(InputAction.CallbackContext value)
    {
        if(!isPermitted || currentWeapon == null) return;

        if(!linkedPhysics.grounded || linkedStateMachine.currentState == PlayerState.State.Dash
        || linkedStateMachine.currentState == PlayerState.State.Knockback
        || linkedStateMachine.currentState == PlayerState.State.Ladder
        || linkedStateMachine.currentState == PlayerState.State.Crouch) return;

        if(!currentWeapon.ReloadCheck() || currentWeapon.isReloading) return;

        reloadCoroutine = StartCoroutine(ReloadProcess());

        
    }

    private IEnumerator ReloadProcess()
    {
        linkedStateMachine.ChangeState(PlayerState.State.Reload);
        currentWeapon.isReloading = true;
        reloadBar.ActivateReloadBar();

        float elapsedTime = 0;
        while(elapsedTime <currentWeapon.reloadTime)
        {
            elapsedTime += Time.deltaTime;
            reloadBar.UpdateReloadBar(elapsedTime, currentWeapon.reloadTime);
            yield return null;
        }

        reloadBar.DeactivateReloadBar();
        currentWeapon.Reload();
        Shooting.OnUpdateAmmo.Invoke(currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);

        if(linkedStateMachine.currentState != PlayerState.State.Death && linkedStateMachine.currentState != PlayerState.State.Knockback)
            linkedStateMachine.ChangeState(PlayerState.State.Idle);
    }

    public override void ExitAbility()
    {
        reloadBar.DeactivateReloadBar();
        if(reloadCoroutine != null)
            StopCoroutine(reloadCoroutine);
        currentWeapon.isReloading = false;
    }
}
