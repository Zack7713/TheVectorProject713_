using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shopkeep : MonoBehaviour
{
    public gameManager manager;
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
        if (other.CompareTag("Player") )
        {
            manager.interactionMenu(); 

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
          manager.closeInteractionMenu();
        }
        
    }
}
