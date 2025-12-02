using UnityEngine;

[CreateAssetMenu(fileName = "DialogueLine", menuName = "Dialouge/DialogueLine")]
public class DialogueLine : ScriptableObject
{
    public string speakerName;
    [TextArea(2,5)]
    public string dialogueText;
    public Sprite speakerIcon;
}
