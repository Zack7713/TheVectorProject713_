using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPos : MonoBehaviour
{
    private Vector3 spawnPosition;

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
}
