using UnityEngine;

public class MoveAbility : BaseAbility
{
    [SerializeField] private float speed = 5f;
    private string runAnimParameterName = "Run";
    private int runParameterID;
    protected override void Initialization()
    {
        base.Initialization();
        runParameterID = Animator.StringToHash(runAnimParameterName);
    }

    public override void EnterAbility()
    {
        player.Flip();
    }
    public override void ProcessAbility()
    {
        if (linkedPhysics.grounded && linkedInput.horizontalInput == 0)
        {
            linkedStateMachine.ChangeState(PlayerState.State.Idle);
        }
        if (!linkedPhysics.grounded)
        {
            linkedStateMachine.ChangeState(PlayerState.State.Jump);
        }
    }
    public override void ProcessFixedAbility()
    {
        linkedPhysics.rb.linearVelocity = new Vector2(linkedInput.horizontalInput * speed, linkedPhysics.rb.linearVelocityY);
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(runParameterID, linkedStateMachine.currentState == PlayerState.State.Run);
    }
}
