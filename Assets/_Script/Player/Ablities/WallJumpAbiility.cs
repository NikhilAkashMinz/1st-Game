using UnityEngine;
using UnityEngine.InputSystem;

public class WallJumpAbiility : BaseAbility
{
    public InputActionReference wallJumpActionRef;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float wallMaxJumpTime;
    private float wallJumpMinimumTime;
    private float wallJumpTimer;

    private void OnEnable()
    {
        wallJumpActionRef.action.performed += TryToWallJump;
    }
    private void OnDisable()
    {
        wallJumpActionRef.action.performed -= TryToWallJump;
    }
    protected override void Initialization()
    {
        base.Initialization();
        wallJumpTimer = wallMaxJumpTime;
    }

    private void TryToWallJump(InputAction.CallbackContext value)
    {

        if(!isPermitted || linkedStateMachine.currentState == PlayerState.State.Knockback ) return;

        if (EvaluateWallJumpConditions())
        {
            linkedStateMachine.ChangeState(PlayerState.State.WallJump);
            wallJumpTimer = wallMaxJumpTime;
            wallJumpMinimumTime = 0.15f; // Minimum time to allow wall jump
            player.ForceFlip();

            if (player.isFacingRight)
                linkedPhysics.rb.linearVelocity = new Vector2(wallJumpForce.x, wallJumpForce.y);
            else
                linkedPhysics.rb.linearVelocity = new Vector2(-wallJumpForce.x, wallJumpForce.y);
        }
    }

    public override void ProcessAbility()
    {
        wallJumpTimer -= Time.deltaTime;
        wallJumpMinimumTime -= Time.deltaTime;

        if (wallJumpMinimumTime < 0 && linkedPhysics.grounded)
        {
            if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState(PlayerState.State.Run);
            else
                linkedStateMachine.ChangeState(PlayerState.State.Idle);
            return;
            
        }


        if (wallJumpTimer <= 0)
        {
            if (linkedPhysics.grounded)
                linkedStateMachine.ChangeState(PlayerState.State.Idle);
            else
                linkedStateMachine.ChangeState(PlayerState.State.Jump);
            return;
        }

        if (wallJumpMinimumTime <= 0 && linkedPhysics.wallDetected)
        {
            linkedStateMachine.ChangeState(PlayerState.State.WallSlide);
            wallJumpTimer = -1;
          
        }
        
    }

    private bool EvaluateWallJumpConditions()
    {
        if (linkedPhysics.grounded || !linkedPhysics.wallDetected) return false;

        return true;
    }
}
