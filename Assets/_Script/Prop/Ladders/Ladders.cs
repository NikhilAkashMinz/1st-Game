using UnityEngine;

public class Ladders : MonoBehaviour
{
    private LadderAbility ladderAbility;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ladderAbility = collision.GetComponent<LadderAbility>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (ladderAbility != null)
        {
            if (ladderAbility.isPermitted)
                ladderAbility.canGoOnLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
         if(ladderAbility != null)
        {
            if (ladderAbility.isPermitted)
                ladderAbility.canGoOnLadder = false;
        }
    }
}
