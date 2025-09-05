using System.Collections;
using UnityEngine;

public class KnockbackAblity : BaseAbility
{
    private Coroutine currentKnockBack;

    public override void ExitAbility()
    {
        currentKnockBack = null;
    }
    
    public void StartSwingKnockBack(float duration, Vector2 force, int direction)
    {
        if(player.playerStats.GetCanTakeDamage() == false) return;
        
        if (currentKnockBack == null)
        {
            currentKnockBack = StartCoroutine(SwingKnockBack(duration, force, direction));
        }
        else
        {
            //StopCoroutine(currentKnockBack);
            //currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));
        }
    }

    public void StartKnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        if (player.playerStats.GetCanTakeDamage() == false) return;

        if (currentKnockBack == null)
        {
            currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));
        }
        else
        {
            StopCoroutine(currentKnockBack);
            currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));
        }
    }
    public IEnumerator KnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        linkedStateMachine.ChangeState(PlayerState.State.Knockback);
        linkedPhysics.ResetVelocity();

        if (transform.position.x >= enemyObject.transform.position.x)
        {
            linkedPhysics.rb.linearVelocity = force;
        }
        else
        {
            linkedPhysics.rb.linearVelocity = new Vector2(-force.x, force.y);
        }
        yield return new WaitForSeconds(duration);

        if (player.playerStats.GetCurrentHealth() > 0)
        {
            if (linkedPhysics.grounded)
            {
                if (linkedInput.horizontalInput != 0)
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
        }
        else
        {
            linkedStateMachine.ChangeState(PlayerState.State.Death);
        }
    }

    public IEnumerator SwingKnockBack(float duration, Vector2 force, int direction)
    {
        linkedStateMachine.ChangeState(PlayerState.State.Knockback);
        linkedPhysics.ResetVelocity();

        force.x *= direction;
        linkedPhysics.rb.linearVelocity = force;

        yield return new WaitForSeconds(duration);

        if (player.playerStats.GetCurrentHealth() > 0)
        {
            if (linkedPhysics.grounded)
            {
                if (linkedInput.horizontalInput != 0)
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
        }
        else
        {
            linkedStateMachine.ChangeState(PlayerState.State.Death);
        }
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool("KnockBack", linkedStateMachine.currentState == PlayerState.State.Knockback);
    }
}
