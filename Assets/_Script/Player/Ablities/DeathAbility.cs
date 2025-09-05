using UnityEngine;

public class DeathAbility : BaseAbility
{

    public override void EnterAbility()
    {
        SpawnMode.spawnFromCheckpoint = true;
        linkedPhysics.ResetVelocity();
        linkedPhysics.rb.linearVelocity = Vector2.zero;
        player.gatherInput.DisablePlayerMap();
        if (linkedPhysics.grounded)
        {
            linkedAnimator.SetBool("Death", true);
        }
        else
        {
            linkedAnimator.SetBool("Death", true);
        }    
    }

    public void ResetGame()
    {
        Debug.Log("Reset Game");
        LevelManager.instance.RestartLevel();

    }
    
}
