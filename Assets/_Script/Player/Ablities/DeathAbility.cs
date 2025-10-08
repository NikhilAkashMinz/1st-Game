using UnityEngine;
using System.IO;

public class DeathAbility : BaseAbility
{

    public override void EnterAbility()
    {
        player.DeactivateCurrentWeapon(); 
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
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);

        if (File.Exists(loadPath))
        {
            CheckpointData checkData = new CheckpointData();
            SaveLoadManager.Instance.Load(checkData, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);
            LevelManager.instance.LoadLevelString(checkData.sceneToLoad);
        }
        else
        {
            LevelManager.instance.RestartLevel();
        }

    }
    
}
