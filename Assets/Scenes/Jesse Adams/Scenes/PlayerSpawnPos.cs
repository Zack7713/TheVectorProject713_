using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPos : MonoBehaviour
{
    private Vector3 spawnPosition;
    private playerController controller;

    // Store the spawn position
    public void SetSpawnPosition(Vector3 position)
    {
        spawnPosition = position;
    }

    // Retrieve the spawn position
    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the playerController component
        //controller = GameObject.FindWithTag("Player").GetComponent<playerController>();

        //// Check if the controller is not null before calling methods
        //if (controller != null)
        //{
        //    SetSpawnPosition(spawnPosition);
        //    GetSpawnPosition();
        //    controller.respawnPlayerOnLoad(spawnPosition);
        //}
        //else
        //{
        //    Debug.LogError("Player controller not found!");
        //}
    }
}