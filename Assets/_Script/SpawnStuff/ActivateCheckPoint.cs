using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateCheckPoint : MonoBehaviour
{
    public InputActionReference AtivateCheck;
    [HideInInspector]
    public Checkpoint checkPoint;

    private void OnEnable()
    {
        AtivateCheck.action.performed += TryToActivate;
    }

    private void OnDisable()
    {
        AtivateCheck.action.performed -= TryToActivate;
    }

    private void TryToActivate(InputAction.CallbackContext value)
    {
        if (checkPoint == null) return;

        checkPoint.Activate();
    }
}
