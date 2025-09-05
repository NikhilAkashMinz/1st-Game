using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    [SerializeField] private string levelToLoad;

    public SpawnData spawnDataForOtherLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SaveLoadManager.Instance.Save(spawnDataForOtherLevel, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileName);
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.gatherInput.DisablePlayerMap();
            player.physicsControl.ResetVelocity();
            LevelManager.instance.LoadLevelString(levelToLoad);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
