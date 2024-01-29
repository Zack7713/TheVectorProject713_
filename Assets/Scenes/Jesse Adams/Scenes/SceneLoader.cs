using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int[] sceneIndices;
    PlayerSpawnPos playerSpawnPos;
    playerController controller; 
    void Start()
    {
         playerSpawnPos = FindObjectOfType<PlayerSpawnPos>();
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
                playerSpawnPos.SetSpawnPosition(transform.position);
                playerController.Instantiate(playerSpawnPos);
           
            controller = FindObjectOfType<playerController>();
        



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
