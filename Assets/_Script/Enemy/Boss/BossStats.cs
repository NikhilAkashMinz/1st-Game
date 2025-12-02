using UnityEngine;

public class BossStats : EnemyStats
{
    [SerializeField] private BossStateMachine bossStateMachine;
    [SerializeField] private HealthBarControl bossHealthBar;

    protected override void DamageProcess()
    {
        bossHealthBar.SetSliderValue(health, maxHealth);
    }

    protected override void DeathProcess()
    {
        bossStateMachine.ChangeState(BossStateMachine.BossState.Death); 
    }
}
