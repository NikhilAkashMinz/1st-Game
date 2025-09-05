using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchAbility : BaseAbility
{
    public InputActionReference crouchActionRef;
    [SerializeField] private float crouchSpeed;
    private string crouchParameterName = "Crouch";
    private string xSpeedParamenterName = "xSpeed";
    private int xSpeedParamenterID;
    private int crouchParameterID;
    private bool wanttoStop;


    protected override void Initialization()
    {
        base.Initialization();
        crouchParameterID = Animator.StringToHash(crouchParameterName);
        xSpeedParamenterID = Animator.StringToHash(xSpeedParamenterName);
    }
    private void OnEnable()
    {
        crouchActionRef.action.performed += TryToCrouch;
        crouchActionRef.action.canceled += StopCrouch;
    }

    private void OnDisable()
    {
        crouchActionRef.action.performed -= TryToCrouch;
        crouchActionRef.action.canceled -= StopCrouch;
    }

    public override void EnterAbility()
    {
        linkedPhysics.CrouchCollider();
        player.playerStats.EnableStatsCrouchCol();
    }

    public override void ExitAbility()
    {
        wanttoStop = false;
        linkedPhysics.StandCollider();
        player.playerStats.EnableStatsStandCol();
    }

    private void TryToCrouch(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerState.State.Knockback ) return;

        if (linkedPhysics.grounded == false || linkedStateMachine.currentState == PlayerState.State.Dash || linkedStateMachine.currentState == PlayerState.State.Ladder) return;

        wanttoStop = false;
        linkedStateMachine.ChangeState(PlayerState.State.Crouch);
    }

    private void StopCrouch(InputAction.CallbackContext value)
    {
        if (!isPermitted) return;

        if (linkedStateMachine.currentState != PlayerState.State.Crouch) return;

        if (linkedPhysics.cellingDetected)
        {
            wanttoStop = true;
            return;
        }
        if (linkedInput.horizontalInput == 0)
            linkedStateMachine.ChangeState(PlayerState.State.Idle);
        else if (linkedInput.horizontalInput != 0)
            linkedStateMachine.ChangeState(PlayerState.State.Run);

    }

    public override void ProcessAbility()
    {
        player.Flip();

        if (wanttoStop && linkedPhysics.cellingDetected == false)
        {
            if (linkedInput.horizontalInput == 0)
                linkedStateMachine.ChangeState(PlayerState.State.Idle);
            else if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState(PlayerState.State.Run);
        }
        if (!linkedPhysics.grounded)
                linkedStateMachine.ChangeState(PlayerState.State.Jump);
    }

    public override void ProcessFixedAbility()
    {
        if (linkedPhysics.grounded)
            linkedPhysics.rb.linearVelocity = new Vector2(linkedInput.horizontalInput * crouchSpeed, linkedPhysics.rb.linearVelocityY);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(crouchParameterID, linkedStateMachine.currentState == PlayerState.State.Crouch);
        linkedAnimator.SetFloat(xSpeedParamenterID, MathF.Abs(linkedPhysics.rb.linearVelocityX));
    }

}
