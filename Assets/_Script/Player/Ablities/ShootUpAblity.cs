using UnityEngine;
using UnityEngine.InputSystem;

public class ShootUpAblity : BaseAbility
{
    public InputActionReference shootUpRef;
    public Weapon currentWeapon;
    [SerializeField] private float airSpeed = 0.5f;
    [SerializeField]private bool shootingUpActivated;


    public void OnEnable()
    {
        shootUpRef.action.performed += TryToShootUp;
        shootUpRef.action.canceled += StopShootUp;
    }

    public void OnDisable()
    {
        shootUpRef.action.performed -= TryToShootUp;
        shootUpRef.action.canceled -= StopShootUp;
    }

    public override void EnterAbility()
    {
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
        player.SetUpShootPos(); 
    }

    public override void ExitAbility()
    {
        shootingUpActivated = false;
        player.SetStandShootPos();
    }

    protected override void Initialization()
    {
        base.Initialization();
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
    }

    public override void ProcessFixedAbility()
    {
        if(!linkedPhysics.grounded)
        {
            linkedPhysics.rb.linearVelocity = new Vector2(linkedInput.horizontalInput * airSpeed, linkedPhysics.rb.linearVelocityY);
        }
        else
        {
            linkedPhysics.rb.linearVelocity = Vector2.zero;
        }
    }

    private void TryToShootUp(InputAction.CallbackContext value)
    {
        if(isPermitted == false || currentWeapon == null) { return; }
        if(linkedStateMachine.currentState == PlayerState.State.Ladder ||
        linkedStateMachine.currentState == PlayerState.State.WallJump ||
        linkedStateMachine.currentState == PlayerState.State.WallSlide ||
        linkedStateMachine.currentState == PlayerState.State.Crouch ||
        linkedStateMachine.currentState == PlayerState.State.Reload ||
        linkedStateMachine.currentState == PlayerState.State.Knockback ||
        linkedStateMachine.currentState == PlayerState.State.Dash) return;

        linkedStateMachine.ChangeState(PlayerState.State.ShootUp);
        shootingUpActivated = true;

        if(linkedPhysics.grounded)
        {
            linkedPhysics.ResetVelocity();
        }
    }
    private void StopShootUp(InputAction.CallbackContext value)
    {
        if(!shootingUpActivated) { return; }

        if(linkedPhysics.grounded)
        {
            if(linkedInput.horizontalInput != 0)
            {
                linkedStateMachine.ChangeState(PlayerState.State.Run);
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerState.State.Idle);
            }
        }
        else
        {
            linkedStateMachine.ChangeState(PlayerState.State.Jump);
        }

        shootingUpActivated = false;
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool("ShootUp", linkedStateMachine.currentState == PlayerState.State.ShootUp);
    }
}
