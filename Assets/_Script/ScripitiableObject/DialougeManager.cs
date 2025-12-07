using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialougeManager : MonoBehaviour
{
   public static DialougeManager dialougeManagerInstance;
   private GatherInput gatherInput;

   [Header("Dialouge UI Elements")]
   [SerializeField] private GameObject dialougeUI;
   [SerializeField] private Image speakerImage;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialougeText;
    [SerializeField] private float textSpeed = 0.02f;

    private DialougeObject currentDialouge;
    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;

    public void RegisterGatherInput(GatherInput gatherInputInstance)
    {
        gatherInput = gatherInputInstance;
    }

    private void Awake()
    {
        if (dialougeManagerInstance == null)
        {
            dialougeManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }




    public void StartDialouge(DialougeObject dialouge)
    {
        currentDialouge = dialouge;
        currentLineIndex = 0;
        dialougeUI.SetActive(true);
        gatherInput.DialogueActive();
        Interact.isInteracting = true;
        ShowLine();
    }

    private void ShowLine()
    {
        DialogueLine line = currentDialouge.lines[currentLineIndex];
        speakerNameText.text = line.speakerName;
        speakerImage.sprite = line.speakerIcon;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine(line.dialogueText));
    }

    private void NextLine()
    {
        currentLineIndex++;
        if(currentLineIndex >= currentDialouge.lines.Length)
        {
            EndDialouge();
        }
        else
        {
            ShowLine();
        }
    }

    public void ContinueDialouge()
    {
        if (isTyping)
        {
            FinishTyping();
        }
        else
        {
            NextLine();
        }
    }

    private void FinishTyping()
    {
        StopCoroutine(typingCoroutine);
        dialougeText.text = currentDialouge.lines[currentLineIndex].dialogueText;
        isTyping = false;
    }

    private void EndDialouge()
    {
        dialougeUI.SetActive(false);
        currentDialouge = null;
        gatherInput.DialogueNotActive();
        Interact.isInteracting = false;
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialougeText.text = "";
        foreach(char c in line)
        {
            dialougeText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }
}
