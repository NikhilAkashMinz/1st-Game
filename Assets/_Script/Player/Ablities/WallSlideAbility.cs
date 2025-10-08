using UnityEngine;

public class WallSlideAbility : BaseAbility
{
    [SerializeField] private float maxSlideSpeed;
    private string wallSlideAnimParameterName = "WallSlide";
    private int wallSlideParameterID;

    protected override void Initialization()
    {
        base.Initialization();
        wallSlideParameterID = Animator.StringToHash(wallSlideAnimParameterName);
    }
    public override void EnterAbility()
    {
        linkedPhysics.rb.linearVelocity = Vector2.zero; // Stop any movement when entering wall slide
        player.DeactivateCurrentWeapon();
    }
    public override void ExitAbility()
    {
        player.ActivateCurrentWeapon();
    }
    public override void ProcessAbility()
    {
        if (linkedPhysics.grounded)
        {
            linkedStateMachine.ChangeState(PlayerState.State.Idle);
            return;
        }
        if (player.isFacingRight && linkedInput.horizontalInput < 0)
        {
            linkedStateMachine.ChangeState(PlayerState.State.Jump);
            return;
        }
        if (!player.isFacingRight && linkedInput.horizontalInput > 0)
        {
            linkedStateMachine.ChangeState(PlayerState.State.Jump);
            linkedPhysics.wallDetected = false; // Reset wall detection when jumping
            linkedAnimator.SetBool("WallSlide", false);
            return;
        }
        if (!linkedPhysics.wallDetected)
        {
            linkedStateMachine.ChangeState(PlayerState.State.Jump);
            linkedPhysics.wallDetected = false; // Reset wall detection when jumping
            linkedAnimator.SetBool("WallSlide", false);
            return;
        }
    }
    public override void ProcessFixedAbility()
    {
        linkedPhysics.rb.linearVelocityY = Mathf.Clamp(linkedPhysics.rb.linearVelocityY, -maxSlideSpeed, 1);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(wallSlideParameterID, linkedStateMachine.currentState == PlayerState.State.WallSlide);
    }
}
