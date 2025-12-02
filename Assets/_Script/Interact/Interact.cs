using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
   public InputActionReference interactActionRef;


   private IInteractable currentInteractable;
   [SerializeField]private GameObject InteractButton;

   private void OnEnable()
   {
       interactActionRef.action.performed += TryToInteract;
   }

   private void OnDisable()
   {
       interactActionRef.action.performed -= TryToInteract;
   }

   private void TryToInteract(InputAction.CallbackContext value)
   {
        
       if(currentInteractable != null)
       {
           currentInteractable.CustomInteract();
       }
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
        InteractButton.SetActive(true);
       if(collision.TryGetComponent(out IInteractable interactable))
       {
           currentInteractable = interactable;
       }
   }

   private void OnTriggerExit2D(Collider2D collision)
   {
        InteractButton.SetActive(false);
       if (collision.TryGetComponent(out IInteractable interactable))
       {
           if(currentInteractable == interactable)
           {
               currentInteractable = null;
           }
       }
   }
}
