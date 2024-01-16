using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int[] sceneIndices;

    void Start()
    {
    }

    void Update()
    {
    }

    public void LoadScene(int index)
    {
        if (index >= 0 && index < sceneIndices.Length)
        {
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
