using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;

    private InputActionMap playerMap;
    private InputActionMap uiMap;
    private InputActionMap dialogueMap;

    public InputActionReference jumpActionRef;
    public InputActionReference moveActionRef;
    public InputActionReference verticialActionRef;
    public InputActionReference dialogueActionRef;

    [HideInInspector]
    public float verticalInput;

    [HideInInspector]
    public float horizontalInput;

    private void OnEnable()
    {
        dialogueActionRef.action.performed +=  TryToContinueDialogue;
    }

    private void OnDisable()
    {
        dialogueActionRef.action.performed -= TryToContinueDialogue;
       // playerMap.Disable();
    }

    private void TryToContinueDialogue(InputAction.CallbackContext value)
    {
        DialougeManager.dialougeManagerInstance.ContinueDialouge();
    }

    void Start()
    {
        playerMap = playerInput.actions.FindActionMap("Player");
        uiMap = playerInput.actions.FindActionMap("UI");
        playerMap.Enable();
        dialogueMap = playerInput.actions.FindActionMap("DialogueControl");
        DialougeManager.dialougeManagerInstance.RegisterGatherInput(this);
    }

    void Update()
    {
        horizontalInput = moveActionRef.action.ReadValue<float>();
        verticalInput = verticialActionRef.action.ReadValue<float>();

        // Debugging the inputs
//        Debug.Log("Vertical Input: " + verticalInput);
       // Debug.Log("Horizontal Input: " + horizontalInput);
    }
    public void EnablePlayerMap()
    {
        playerMap.Enable();
    }
    public void DisablePlayerMap()
    {
        playerMap.Disable();
    }

    public void DialogueActive()
    {
        DisablePlayerMap();
        dialogueMap.Enable();
    }

    public void DialogueNotActive()
    {
        EnablePlayerMap();
        dialogueMap.Disable();
    }
}
