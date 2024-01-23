using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public List<gunStats> gunList = new List<gunStats>();
    public int pointAmount;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
