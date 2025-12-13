using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
public class MenuControl : MonoBehaviour
{

    [SerializeField] private GameObject continueButton;
    private void Start()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);
        if (File.Exists(loadPath))
        {
            continueButton.SetActive(true);
            EventSystem.current.firstSelectedGameObject = continueButton;
        }
        else
        {
            continueButton.SetActive(false);
        }
    }
    public void NewGame()
    {
        SaveLoadManager.Instance.DeleteFolder(SaveLoadManager.Instance.folderName);
        LevelManager.instance.LoadLevelString("Level 1");
    }

    public void ContinueGame()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);
        SpawnMode.spawnFromCheckpoint = true;
        if(File.Exists(loadPath))
        {
            CheckpointData helpCheck = new CheckpointData();
            SaveLoadManager.Instance.Load(helpCheck,SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);
            LevelManager.instance.LoadLevelString(helpCheck.sceneToLoad);
        }
        else
        {
            CheckpointData helpCheck = new CheckpointData();
            SaveLoadManager.Instance.Save(helpCheck,SaveLoadManager.Instance.folderName, SaveLoadManager.Instance.fileCheckPoint);
            LevelManager.instance.LoadLevelString(helpCheck.sceneToLoad);
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
