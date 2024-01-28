using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPostFlag : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
 
        if (other.CompareTag("Player"))
        {

            gameManager.instance.RoundStartPrompt();

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.closeInteractionMenu();
        }

    }
}
