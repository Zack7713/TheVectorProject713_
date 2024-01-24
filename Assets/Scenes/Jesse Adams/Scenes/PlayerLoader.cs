using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public static PlayerLoader instance;

    // Variables to store persistent data
    public int points;
    //what if instead of storing gun names we store the gunstats?
    public List<string> acquiredGuns; // Store gun names or IDs
    //public List<gunStats> acquiredGuns = new List<gunStats>();

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
