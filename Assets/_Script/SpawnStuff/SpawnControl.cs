using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnControl : MonoBehaviour
{
    private Transform player;

    [SerializeField] private SpawnIdentifier[] spawnPoints;
    [SerializeField] private SpawnIdentifier[] spawncheckPoints;

    private CheckpointData checkData = new CheckpointData();
    private SpawnData spawnData = new SpawnData();

    private bool canLoadFromCheckpoint = false;
    void Start()
    {
        player = FindAnyObjectByType<Player>().transform;
        // Load checkpoint data
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);

        if (File.Exists(loadPath))
        {
            SaveLoadManager.Instance.Load(checkData, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);
            if(checkData.sceneToLoad == SceneManager.GetActiveScene().name)
            {
                canLoadFromCheckpoint = true;
            }
        }

        if (SpawnMode.spawnFromCheckpoint && canLoadFromCheckpoint == true)
        {
            foreach (SpawnIdentifier spawnID in spawncheckPoints)
            {
                if (spawnID.spawnKey == checkData.checkpointKey)
                {
                    player.transform.position = spawnID.transform.position;
                    break;
                }
            }
            if (checkData.facingRight == false)
            {
                player.GetComponent<Player>().ForceFlip();

            }
            SpawnMode.spawnFromCheckpoint = false;

        }
        else
        {
            // SaveLoadManager.Instance.DeleteFolder(SaveLoadManager.Instance.folderName);
            SaveLoadManager.Instance.Load(spawnData, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileName);
            foreach (SpawnIdentifier spawnID in spawnPoints)
            {
                if (spawnID.spawnKey == spawnData.spawnPointKey)
                {
                    player.transform.position = spawnID.transform.position;
                    break;
                }
            }
            if (spawnData.facingRight == false)
            {
                player.GetComponent<Player>().ForceFlip();
            }
        }
        
    }
}
