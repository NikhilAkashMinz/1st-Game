 using UnityEngine;

public class IdelAblity : BaseAbility
{
    private string idleAnimParameterName = "Idle";
    private int idleParameterInt;



    public override void EnterAbility()
    {
        linkedPhysics.rb.linearVelocityX = 0;
    }
    protected override void Initialization()
    {
        base.Initialization();
        idleParameterInt = Animator.StringToHash(idleAnimParameterName);
    }
    public override void ProcessAbility()
    {
        //Debug.Log("Processing Idle Ability");
        if (linkedInput.horizontalInput != 0)
        {
            player.Flip();
            linkedStateMachine.ChangeState(PlayerState.State.Run);
        }
    }
    public override void FixedUpdateAbility()
    {
        linkedPhysics.rb.linearVelocityX = 0;
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(idleParameterInt, linkedStateMachine.currentState == PlayerState.State.Idle || linkedStateMachine.currentState == PlayerState.State.Reload);
    }
}
