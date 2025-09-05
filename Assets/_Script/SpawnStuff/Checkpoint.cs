using UnityEngine;
using UnityEngine.UIElements;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite spriteDisable;
    [SerializeField] private Sprite spriteEnable;
    [SerializeField] private BoxCollider2D boxCol;
    [SerializeField] private CheckpointData checkpointData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sprite.sprite = spriteEnable;

            // Save checkpoint data
            SaveLoadManager.Instance.Save(checkpointData, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);
             SpawnMode.spawnFromCheckpoint = true;
        }
    }
}