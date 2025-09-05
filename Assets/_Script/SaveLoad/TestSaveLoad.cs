using UnityEngine;

public class TestSaveLoad : MonoBehaviour
{
    public ExampleData someData;

    private void Start()
    {
        SaveLoadManager.Instance.DeleteExampleSave("test.json");
        SaveLoadManager.Instance.LoadExample(someData, "test.json");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        someData.exampleInt = 10;
        someData.exampleString = "Hello World!";
        SaveLoadManager.Instance.SaveExample(someData, "test.json");
    }
}
