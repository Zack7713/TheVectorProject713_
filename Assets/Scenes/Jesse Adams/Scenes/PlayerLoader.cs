using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public static PlayerLoader instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
