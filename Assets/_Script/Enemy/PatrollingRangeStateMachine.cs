using UnityEngine;
using System.Collections;

public class PatrollingRangeStateMachine : EnemySimpleStateMachine 
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
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float rayLength;
    [SerializeField] private float damage;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float visibleLineTime;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private LayerMask whatToHit;


    [Header("Death State")]
    [SerializeField] private string deathAnimationName;

    #region IDLE
    public override void EnterIdle()
    {
        anim.Play(idleAnimationName);
        idleStateTimer = Random.Range(minIdleTime, maxIdleTime);
        patrollPhysics.NegateForces();
    }

    public override void UpdateIdle()
    {

        if (patrollPhysics.playerBehind)
        {
            ForceFlip();
            speed *= -1;
            turnCooldown = minimumTurnDelay;
            ChangeState(EnemyState.Move);
        }
        
        idleStateTimer -= Time.deltaTime;
        if (idleStateTimer <= 0f || patrollPhysics.playerAhead==false)
        {
            ChangeState(EnemyState.Move);
        }

        if (patrollPhysics.playerAhead)
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
        moveStateTimer = Random.Range(minMoveTime, maxMoveTime);
    }

    public override void UpdateMove()
    {
        moveStateTimer -= Time.deltaTime;

        if (moveStateTimer <= 0 && patrollPhysics.playerAhead == false)
        {
            ChangeState(EnemyState.Idle);
        }

        if (turnCooldown > 0)
        {
            turnCooldown -= Time.deltaTime;
        }
        if(patrollPhysics.playerBehind && turnCooldown <= 0)
        {
            ForceFlip();
            speed *= -1;
            turnCooldown = minimumTurnDelay;
            return;
        }

        if (patrollPhysics.wallDetected || patrollPhysics.groundDetected == false)
        {
            if (turnCooldown > 0)
                return;


            ForceFlip();
            speed *= -1;
            turnCooldown = minimumTurnDelay;
        }

        if (patrollPhysics.playerAhead)
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
        patrollPhysics.canCheckBehind = false;
    }

    public void EndOfAttack()
    {
        if (patrollPhysics == null)
            return;

        if (patrollPhysics.playerAhead)
        {
            anim.Play(attackAnimationName, 0, 0f); // Re-attack if still in range
        }
        else
        {
            ChangeState(EnemyState.Move); // Or return to patrol
        }
        StartCoroutine(CheckBehindDelay());
    }

    IEnumerator CheckBehindDelay()
    {
        yield return new WaitForSeconds(0.5f);
        patrollPhysics.canCheckBehind = true;
    }

    public void ShootAttack()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(shootingPoint.position, transform.right, rayLength, whatToHit);
        lineRenderer.positionCount = 2;
        if (hitInfo)
        {
            lineRenderer.SetPosition(0, shootingPoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            Vector2 normal = hitInfo.normal;
            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(hitEffectPrefab, hitInfo.point, rotation);
            PlayerStats playerStats = hitInfo.collider.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.DamagePlayer(damage);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, shootingPoint.position);
            lineRenderer.SetPosition(1, shootingPoint.position + transform.right * 20);
        }
        StartCoroutine(ResetShootLine());
    }
    private IEnumerator ResetShootLine()
    {
        yield return new WaitForSeconds(visibleLineTime);
        lineRenderer.positionCount = 0;
    }

    #endregion ATTACK

    #region DEATH
    public override void EnterDeath()
    {
        anim.Play(deathAnimationName);
        patrollPhysics.NegateForces();
        patrollPhysics.DeatCollliderDeactivation();
    }

    #endregion DEATH
}



