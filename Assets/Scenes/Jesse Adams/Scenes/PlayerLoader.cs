using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public static PlayerLoader instance;

    // Variables to store persistent data
    public int points;
    public List<string> acquiredGuns; // Store gun names or IDs

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            acquiredGuns = new List<string>(acquiredGuns);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
