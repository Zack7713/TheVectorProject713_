using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    public float speed = 10f; // how fast to scan
    public float angle = 45f; // how much to scan

    private void Update()
    {
        // calculate the rotation angle using PingPong
        float rotation = Mathf.PingPong(Time.time * speed, angle * 2) - angle;

        // create a rotation with the specified angle around the y-axis
        Quaternion rot = Quaternion.Euler(0, rotation, 0);

        // apply the rotation to the object
        transform.rotation = rot;
    }
}
