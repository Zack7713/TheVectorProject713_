using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubKeep : MonoBehaviour
{
 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.interactionHubMenu();

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.closeInteractionHubMenu();
        }

    }
}
