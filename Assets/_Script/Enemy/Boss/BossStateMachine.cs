using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
     protected BossState previousState;
    protected BossState currentState;

    [SerializeField] protected Animator anim;
    public bool facingRight = true;

    public void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }


    public enum BossState
    {
        Idle,
        Teleport,
        Attack,
        RangeAttack,
        Summon,
        Death,

    }

    public void ChangeState(BossState newState)
    {
        if(newState == currentState)
            return;

        ExitState(currentState);
        previousState = currentState;
        currentState = newState;
        EnterState(currentState);
    }

    private void Update()
    {
        switch(currentState)
        {
            case BossState.Idle:
                UpdateIdle();
                break;
            case BossState.Teleport:
                UpdateTeleport();
                break;
            case BossState.Attack:
                UpdateAttack();
                break;
            case BossState.RangeAttack:
                UpdateRangeAttack();
                break;
            case BossState.Summon:
                UpdateSummon();
                break;
            case BossState.Death:
                UpdateDeath();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch(currentState)
        {
            case BossState.Idle:
                FixUpdateIdle();
                break;
            case BossState.Teleport:
                FixUpdateTeleport();
                break;
            case BossState.Attack:
                FixUpdateAttack();
                break;
            case BossState.RangeAttack:
                FixUpdateRangeAttack();
                break;
            case BossState.Summon:
                FixUpdateSummon();
                break;
            case BossState.Death:
                FixUpdateDeath();
                break;
        }
        
    }

    protected void EnterState(BossState state)
    {
        switch(state)
        {
            case BossState.Idle:
                EnterIdle();
                break;
            case BossState.Teleport:
                EnterTeleport();
                break;
            case BossState.Attack:
                EnterAttack();
                break;
            case BossState.RangeAttack:
                UpdateRangeAttack();
                break;
            case BossState.Summon:
                UpdateSummon();
                break;
            case BossState.Death:
                EnterDeath();
                break;
        }
    }

    protected void ExitState(BossState state)
    {
        switch(state)
        {
            case BossState.Idle:
                ExitIdle();
                break;
            case BossState.Teleport:
                ExitTeleport();
                break;
            case BossState.Attack:
                ExitAttack();
                break;
            case BossState.RangeAttack:
                ExitRangeAttack();
                break;
            case BossState.Summon:
                ExitSummon();
                break;
            case BossState.Death:
                ExitDeath();
                break;
        }
    }

    public virtual void EnterIdle()
    {
        
    }
    public virtual void EnterTeleport()
    {
        
    }
    public virtual void EnterAttack()
    {

    }
    public virtual void EnterRangeAttack()
    {

    }
    public virtual void EnterSummon()
    {

    }
    public virtual void EnterDeath()
    {
        
    }



    public virtual void UpdateIdle()
    {
        
    }       
    public virtual void UpdateTeleport()
    {
        
    }

    public virtual void UpdateAttack()
    {

    }

    public virtual void UpdateRangeAttack()
    {

    }

    public virtual void UpdateSummon()
    {
        
    }
    public virtual void UpdateDeath()
    {
        
    }

    public virtual void FixUpdateIdle()
    {
        
    }
    public virtual void FixUpdateTeleport()
    {
        
    }
    public virtual void FixUpdateAttack()
    {

    }

    public virtual void FixUpdateRangeAttack()
    {

    }

    public virtual void FixUpdateSummon()
    {

    }
    
    public virtual void FixUpdateDeath()
    {
        
    }




    public virtual void ExitIdle()
    {
        
    }
    public virtual void ExitTeleport()
    {
        
    }
    public virtual void ExitAttack()
    {

    }

    public virtual void ExitRangeAttack()
    {

    }
    
    public virtual void ExitSummon()
    {
        
    }
    public virtual void ExitDeath()
    {
        
    }
}
