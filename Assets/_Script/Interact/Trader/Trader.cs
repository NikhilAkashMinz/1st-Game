using UnityEngine;

public class Trader : MonoBehaviour,IInteractable
{
   public void CustomInteract()
   {
       Debug.Log("Trader Interacted");
   }
}
