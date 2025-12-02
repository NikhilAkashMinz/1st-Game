using UnityEngine;

public class FirstBossStateMachine : BossStateMachine
{
    [SerializeField] private BossPhysics bossPhysics;
    private Player player;

    [Header("Idle State")]
    [SerializeField] private string idleAnimationName;
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    private float idleStateTimer;

    [Header("Teleport State")]
    [SerializeField] private string teleportOutAnimationName;
    [SerializeField] private string teleportInAnimationName;
    [SerializeField] private float minTeleportTime;
    [SerializeField] private float maxTeleportTime;
    [SerializeField] private Transform[] teleportPoints;
    private float teleportStateTimer;
    private int teleportIndex;
    private int lastTeleportIndex;
    private bool canCheckTeleportInfo;

    [Header("Attack State")]
    [SerializeField] private string attackAnimationName;
    [SerializeField] private float attackMeleeCoolDownTime;
    private float meleeAttackTimer;

    [Header("Range Attack State")]
    [SerializeField] private string attackRangeAnimationName;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootingPoint;

    [Header("Death State")]
    [SerializeField] private string deathAnimationName;
    [SerializeField]private GameObject headPrefab;
    [SerializeField]private GameObject sakshan;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();


    }



    #region IDLE

    public override void EnterIdle()
    {
        anim.Play(idleAnimationName);
        idleStateTimer = Random.Range(minIdleTime, maxIdleTime);
        //bossPhysics.NegateForces();
    }

    public override void UpdateIdle()
    {
        meleeAttackTimer -= Time.deltaTime;
        if (bossPhysics.inAttackRange)
        {
            if (meleeAttackTimer <= 0f)
            {
                ChangeState(BossState.Attack);
            }
            return;
        }


        idleStateTimer -= Time.deltaTime;
        if (idleStateTimer <= 0f)
        {
            ChangeState(BossState.Teleport);
        }
    }



    #endregion IDLE

    #region Teleport

    public override void EnterTeleport()
    {
        bossPhysics.DisableStatsCol();
        teleportIndex = Random.Range(0, teleportPoints.Length);

        while (teleportIndex == lastTeleportIndex)
        {
            teleportIndex = Random.Range(0, teleportPoints.Length);
        }
        lastTeleportIndex = teleportIndex;
        anim.Play(teleportOutAnimationName);
    }

    public override void UpdateTeleport()
    {
        if (!canCheckTeleportInfo)
            return;

        teleportStateTimer -= Time.deltaTime;
        if (bossPhysics.inAttackRange)
        {
            ChangeState(BossState.Attack);
        }
        else if (teleportStateTimer <= 0f)
        {
            ChangeState(BossState.RangeAttack);
        }
    }

    public override void ExitTeleport()
    {
        canCheckTeleportInfo = false;
    }

    public void Teleport()
    {
        int randomChance = Random.Range(0, 2);
        if (randomChance == 0)
        {
            transform.position = teleportPoints[teleportIndex].position;
        }
        else
        {
            if (player != null)
                transform.position = player.transform.position + Vector3.up * 1.6f;
            else
                transform.position = teleportPoints[teleportIndex].position;
        }
        anim.Play(teleportInAnimationName);
    }

    public void EnableCheckingTeleport()
    {
        canCheckTeleportInfo = true;
        teleportStateTimer = Random.Range(minTeleportTime, maxTeleportTime);
        bossPhysics.EnableStatsCol();
        bossPhysics.EnableDetectionCol();
        anim.Play(idleAnimationName);
    }
    #endregion

    #region ATTACK

    public override void EnterAttack()
    {
        anim.Play(attackAnimationName);
        bossPhysics.DisableDetectionCol();
        bossPhysics.inAttackRange = false;
    }

    public override void ExitAttack()
    {
        meleeAttackTimer = attackMeleeCoolDownTime;
        bossPhysics.EnableDetectionCol();
    }

    public void ChangeStateToIdle()
    {

        ChangeState(BossState.Idle);

    }

    #endregion ATTACK

    #region RANGE ATTACK

    public override void EnterRangeAttack()
    {
        Debug.Log("Boss entered RangeAttack");
        anim.Play(attackRangeAnimationName);
    }

    public void SpawnBossProjectile()
    {
        BossProjectile projectile = Instantiate(projectilePrefab, shootingPoint.position, transform.rotation).GetComponent<BossProjectile>();
        if (player != null)
        {
            projectile.MoveProjectile(player.transform);
        }
        else
        {
            Destroy(projectile.gameObject);
        }
    }



    #endregion RANGE ATTACK

    #region DEATH
        public override void EnterDeath()
        {
            anim.Play(deathAnimationName);
            bossPhysics.DisableAllCol();
        }

        public void DeathAnimationEvent()
        {
            Instantiate(headPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
            sakshan.SetActive(false);
        }
    #endregion DEATH
}
