using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;

    private InputActionMap playerMap;
    private InputActionMap uiMap;
    public InputActionReference jumpActionRef;
    public InputActionReference moveActionRef;

    public InputActionReference verticialActionRef;

    [HideInInspector]
    public float verticalInput;

    [HideInInspector]
    public float horizontalInput;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

       // playerMap.Disable();
    }

    void Start()
    {
        playerMap = playerInput.actions.FindActionMap("Player");
        uiMap = playerInput.actions.FindActionMap("UI");
        playerMap.Enable();
    }

    void Update()
    {
        horizontalInput = moveActionRef.action.ReadValue<float>();
        verticalInput = verticialActionRef.action.ReadValue<float>();

        // Debugging the inputs
//        Debug.Log("Vertical Input: " + verticalInput);
       // Debug.Log("Horizontal Input: " + horizontalInput);
    }
    public void DisablePlayerMap()
    {
        playerMap.Disable();
    }
}
