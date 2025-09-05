using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the climbing ability of the player.
/// </summary>

public class LadderAbility : BaseAbility
{

    public InputActionReference ladderActionRef;
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float setMinLadderTime;
    private float minimumLadderTime;
    private bool climb;
    public bool canGoOnLadder;
    private string ladderParameterName = "Ladder";
    private int ladderParameterID;

    protected override void Initialization()
    {
        base.Initialization();
        minimumLadderTime = setMinLadderTime;
        ladderParameterID = Animator.StringToHash(ladderParameterName);
    }

    private void OnEnable()
    {
        ladderActionRef.action.performed += TryToClimb;
        ladderActionRef.action.canceled += StopClimb;
    }
    private void OnDisable()
    {
        ladderActionRef.action.performed -= TryToClimb;
        ladderActionRef.action.canceled -= StopClimb;
    }


    private void TryToClimb(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerState.State.Knockback) return;
        linkedAnimator.enabled = true;

        if (linkedStateMachine.currentState == PlayerState.State.Ladder || linkedStateMachine.currentState == PlayerState.State.Dash || !canGoOnLadder) return;

        linkedStateMachine.ChangeState(PlayerState.State.Ladder);
        linkedPhysics.DisableGravity();
        linkedPhysics.ResetVelocity();
        climb = true;

        minimumLadderTime = setMinLadderTime;
        

    }

    private void StopClimb(InputAction.CallbackContext value)
    {
        if (!isPermitted) return;
        if (linkedStateMachine.currentState != PlayerState.State.Ladder) return;

        linkedPhysics.ResetVelocity();
        linkedAnimator.enabled = false;
    }
    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity();
        linkedAnimator.enabled = true;
        climb = false;
    }

    public override void ProcessAbility()
    {
        if (climb) minimumLadderTime -= Time.deltaTime;

        if (linkedInput.horizontalInput != 0)
        {
            linkedStateMachine.ChangeState(PlayerState.State.Jump);
            return;
        }

        if (!canGoOnLadder)
        {
            if (!linkedPhysics.grounded)
            {
                linkedStateMachine.ChangeState(PlayerState.State.Jump);
            }
        }

        if (linkedPhysics.grounded && minimumLadderTime <= 0)
        {
            linkedStateMachine.ChangeState(PlayerState.State.Idle);

        }
    }

    public override void ProcessFixedAbility()
    {
        if (climb)
        {
            linkedPhysics.rb.linearVelocity = new Vector2(0, linkedInput.verticalInput * climbSpeed);
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(ladderParameterID, linkedStateMachine.currentState  == PlayerState.State.Ladder);
    }
}
