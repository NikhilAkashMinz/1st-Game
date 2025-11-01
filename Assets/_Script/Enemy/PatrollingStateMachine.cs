using UnityEngine;

public class PatrollingStateMachine : EnemySimpleStateMachine
{
    [SerializeField] private PatrollPhysics patrollPhysics;

    [Header("Idle State")]
    [SerializeField] private string idleAnimationName;
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    private float idleStateTimer;

    [Header("Move State")]
    [SerializeField] private string moveAnimationName;
    [SerializeField] private float speed;
    [SerializeField] private float minMoveTime;
    [SerializeField] private float maxMoveTime;
    [SerializeField] private float minimumTurnDelay;
    private float moveStateTimer;
    private float turnCooldown;

    [Header("Attack State")]
    [SerializeField] private string attackAnimationName;

#region IDLE
   public override void EnterIdle()
   {
        anim.Play(idleAnimationName);
        idleStateTimer = Random.Range (minIdleTime, maxIdleTime);
        patrollPhysics.NegateForces();
   }

   public override void UpdateIdle()
   {
        idleStateTimer -= Time.deltaTime;
        if(idleStateTimer <= 0f)
        {
            ChangeState(EnemyState.Move);
        }
        
        if(patrollPhysics.inAttackRange)
        {
            ChangeState(EnemyState.Attack);
        }
   } 

   public override void ExitIdle()
   {

   }
#endregion IDLE
#region MOVE
    public override void EnterMove()
    {
        anim.Play(moveAnimationName);
        moveStateTimer = Random.Range (minMoveTime, maxMoveTime);
    }

    public override void UpdateMove()
    {
        moveStateTimer -= Time.deltaTime;

        if(moveStateTimer <= 0)
        {
            ChangeState(EnemyState.Idle);
        }

        if(turnCooldown > 0)
        {
            turnCooldown -= Time.deltaTime;
        }

        if(patrollPhysics.wallDetected || patrollPhysics.groundDetected == false)
        {
            if(turnCooldown > 0)
                return;


            ForceFlip();
            speed *= -1;

            turnCooldown = minimumTurnDelay;
        }

        if(patrollPhysics.inAttackRange)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    public override void FixUpdateMove()
    {
        if (patrollPhysics == null || patrollPhysics.rb == null)
        return; // ✅ Prevent MissingReferenceException

        patrollPhysics.rb.linearVelocity = new Vector2(speed, patrollPhysics.rb.linearVelocity.y);
    }

#endregion MOVE
#region ATTACK
public override void EnterAttack()
{
    anim.Play(attackAnimationName, 0, 0f); // Force restart animation
    patrollPhysics.NegateForces();
}

public void EndOfAttack()
{
    if (patrollPhysics == null)
        return;

    if (patrollPhysics.inAttackRange)
    {
       anim.Play(attackAnimationName, 0, 0f); // Re-attack if still in range
    }
    else
    {
        ChangeState(EnemyState.Move); // Or return to patrol
    }
}
#endregion ATTACK
}

