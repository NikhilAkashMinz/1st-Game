using UnityEngine;
using UnityEngine.InputSystem;


public class DashAbility : BaseAbility
{
    public InputActionReference dashActionRef;
    [SerializeField] private float dashForce;
    [SerializeField] private float maxDashDuration;
    private float dashTimer;

    private string dashAnimParameterName = "Dash";
    private int dashParameterID;

    protected override void Initialization()
    {
        base.Initialization();
        dashParameterID = Animator.StringToHash(dashAnimParameterName);
    }

    private void OnEnable()
    {
        dashActionRef.action.started += TryToDash;
    }

    private void OnDisable()
    {
        dashActionRef.action.started -= TryToDash;
    }
    public override void EnterAbility()
    {

        //player.playerStats.DisableDamage();
        player.playerStats.DisableStatscollider();
    }
    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity();
        linkedPhysics.ResetVelocity();
        //player.playerStats.EnableDamage();
        player.playerStats.EnableStatscollider();
    }

    private void TryToDash(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerState.State.Knockback) return;

        if (linkedStateMachine.currentState == PlayerState.State.Dash || linkedPhysics.wallDetected 
        || linkedStateMachine.currentState == PlayerState.State.Crouch
        || linkedStateMachine.currentState == PlayerState.State.Reload) return;

        linkedStateMachine.ChangeState(PlayerState.State.Dash);
        linkedPhysics.DisableGravity();
        linkedPhysics.ResetVelocity();
        if (player.isFacingRight)
            linkedPhysics.rb.linearVelocityX = dashForce;
        else
            linkedPhysics.rb.linearVelocityX = -dashForce;
        dashTimer = maxDashDuration;
    }

    public override void ProcessAbility()
    {
        dashTimer -= Time.deltaTime;
        if (linkedPhysics.wallDetected) dashTimer = -1;
        if (dashTimer <= 0)
        {
            if (linkedPhysics.grounded)
                linkedStateMachine.ChangeState(PlayerState.State.Idle);
            else
                linkedStateMachine.ChangeState(PlayerState.State.Jump);
        }

    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(dashParameterID, linkedStateMachine.currentState == PlayerState.State.Dash);
    }

}
