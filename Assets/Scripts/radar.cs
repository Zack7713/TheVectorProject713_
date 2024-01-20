using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    public float radarRadius = 20f; // Adjust this based on your game's needs
    public LayerMask detectionLayer;
    public Image radarImage;
    public string playerTag = "Player";

    void Update()
    {
        DetectObjects();
    }

    void DetectObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radarRadius, detectionLayer);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(playerTag))
            {
                // Player detected, update radar display or perform other actions
                UpdateRadarDisplay(col.transform.position);
            }
        }
    }

    void UpdateRadarDisplay(Vector3 targetPosition)
    {

        // Calculate the direction from radar to target player
        Vector3 directionToTarget = targetPosition - transform.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        // Rotate the radar image to point towards the player
        radarImage.transform.rotation = Quaternion.Euler(0f, 0f, -angle);

    }
}
