using UnityEngine;

public class StateMachine
{
    public PlayerState.State previousState;
    public PlayerState.State currentState;
    
    public BaseAbility[] arrayOfAbilities;

    public void ChangeState(PlayerState.State newState)
    {
        foreach (BaseAbility ability in arrayOfAbilities)
        {
            if (ability.thisAbilityState == newState)
            {
                if (!ability.isPermitted)
                {
                    return;
                }
            }
        }
        foreach (BaseAbility ability in arrayOfAbilities)
        {
            if (ability.thisAbilityState == currentState)
            {
                ability.ExitAbility();
                previousState = currentState;
            }
        }
        foreach (BaseAbility ability in arrayOfAbilities)
        {
            if (ability.thisAbilityState == newState)
            {
                if (ability.isPermitted)
                {
                    currentState = newState;
                    ability.EnterAbility();
                }
                break;
            }
        }
    }
    public void ForceChangeState(PlayerState.State newState)
    {
        previousState = currentState;
        currentState = newState;
    }
}
