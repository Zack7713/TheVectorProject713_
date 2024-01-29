using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int[] sceneIndices;
    PlayerSpawnPos playerSpawnPos;
    playerController controller;
    Vector3 spawnplace;

    void Start()
    {
  
        //controller.respawnPlayerOnLoad(spawnplace);
        //gameManager.instance.playerSpawnPos.transform.position = spawnplace;
    }

    void Update()
    {
    }

    public void LoadScene(int index)
    {
        if (index >= 0 && index < sceneIndices.Length)
        {
            // Find the PlayerSpawnPos object in the loaded scene




            // Set the spawn position in the PlayerSpawnPos script
         

            playerSpawnPos = FindObjectOfType<PlayerSpawnPos>();
            spawnplace = playerSpawnPos.GetSpawnPosition();
            gameManager.instance.playerSpawnPos.transform.position = spawnplace;




            // Load the scene
            SceneManager.LoadScene(sceneIndices[index]);

            if (gameManager.instance != null)
            {
                gameManager.instance.stateUnpaused();
            }
        }
        else
        {
            Debug.LogWarning("Invalid scene index");
        }
    }
}
