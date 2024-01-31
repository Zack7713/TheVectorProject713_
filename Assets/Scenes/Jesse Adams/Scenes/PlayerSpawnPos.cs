using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPos : MonoBehaviour
{
    //private gameManager gameManager;
    //private Vector3 spawnPosition;
    //private playerController controller;

    // Store the spawn position
    //public void SetSpawnPosition(Vector3 position)
    //{
    //    spawnPosition = position;
    //}

    //// Retrieve the spawn position
    //public Vector3 GetSpawnPosition()
    //{
    //    return spawnPosition;
    //}

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.playerSpawnPos = gameObject;
        
        // Get the playerController component
        playerController controller = GameObject.FindWithTag("Player").GetComponent<playerController>();

        // check if the controller is not null before calling methods
        if (controller != null)
        {
            //setspawnposition(spawnposition);
            //getspawnposition();
            controller.respawnPlayerOnLoad(gameObject.transform.position);
        }
        else
        {
            Debug.LogError("player controller not found!");
        }
    }
}