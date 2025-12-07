using UnityEngine;

public class Trader : MonoBehaviour,IInteractable
{
    [SerializeField] private DialougeObject traderDialouge;
   public void CustomInteract()
   {
    //    Debug.Log("Trader Interacted");
         DialougeManager.dialougeManagerInstance.StartDialouge(traderDialouge);
   }
}
