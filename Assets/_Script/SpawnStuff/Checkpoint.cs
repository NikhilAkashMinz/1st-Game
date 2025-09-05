using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite spriteDisable;
    [SerializeField] private Sprite spriteEnable;
    [SerializeField] private BoxCollider2D boxCol;
    [SerializeField] private CheckpointData checkpointData;
    [SerializeField] private GameObject button;

    private void Start()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);
        if (File.Exists(loadPath))
        {
            CheckpointData helpCheck = new CheckpointData();
            SaveLoadManager.Instance.Load(helpCheck, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);

            if (helpCheck.checkpointKey == checkpointData.checkpointKey)
            {
                sprite.sprite = spriteEnable;
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //SpawnMode.spawnFromCheckpoint = true;
            //sprite.sprite = spriteEnable;
            // Save checkpoint data
            //SaveLoadManager.Instance.Save(checkpointData, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);
            collision.GetComponent<ActivateCheckPoint>().checkPoint = this;
            button.SetActive(true);
        }

    }
    
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckPoint>().checkPoint = null;
            button.SetActive(false);
        }
    }
    public void Activate()
    {
        SpawnMode.spawnFromCheckpoint = true;
        sprite.sprite = spriteEnable;
        // Save checkpoint data
        SaveLoadManager.Instance.Save(checkpointData, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);
    }
}