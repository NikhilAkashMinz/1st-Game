using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    [Header("Spawn")]
    public string folderName = "SaveFiles";
    public string fileName = "SpawnPoint.json";

    [Header("CheckPoint")]
    public string fileCheckPoint = "CheckPoint.json";
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save<T>(T dataToSave, string folderName, string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        File.WriteAllText(savePath, JsonUtility.ToJson(dataToSave, true));
    }

    public void Load<T>(T dataToLoadInto, string folderName, string fileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(loadPath))
        {
            string result = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(result, dataToLoadInto);
        }
        else
        {
            Debug.LogWarning("Save file not found: " + loadPath);
        }
    }

    public void DeleteSaveFile(string folderName, string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
      
    }

    public void DeleteFolder(string folderName)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
    }

    public void SaveExample(ExampleData dataToSave, string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(savePath, JsonUtility.ToJson(dataToSave, true));
    }

    public void LoadExample(ExampleData dataToLoadInto, string fileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(loadPath))
        {
            string loadDataString = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(loadDataString, dataToLoadInto);
        }
        else
        {
            Debug.LogWarning("Save file not found: " + loadPath);
        }
    }
    
    public void DeleteExampleSave(string fileName)
    {
        string deletePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(deletePath))
        {
            File.Delete(deletePath);
        }
        else
        {
            Debug.LogWarning("No save file to delete at: " + deletePath);
        }
    }
}
