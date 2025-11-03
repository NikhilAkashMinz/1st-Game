using UnityEngine;

public class DemonStats : EnemyStats
{
    [SerializeField] public EnemySimpleStateMachine enemyStateMachine;
    protected override void DamageProcess()
    {
        //enemyStateMachine.ChangeState(EnemySimpleStateMachine.EnemyState.Hurt);
    }
    protected override void DeathProcess()
    {
        enemyStateMachine.ChangeState(EnemySimpleStateMachine.EnemyState.Death);
    }
}
