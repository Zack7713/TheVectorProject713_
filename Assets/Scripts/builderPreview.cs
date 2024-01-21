using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeManager : MonoBehaviour
{
    playerController player; 
    public GameObject barricadePrefab; // Prefab of the barricade
    public GameObject barricadePreview; // Prefab for the preview

    private GameObject previewInstance; // Instance of the preview

    void Update()
    {
        if (Input.GetButtonDown("Utility")) // Replace with the button you want to use for preview
        {
            TogglePreviewMode();
        }

        if (Input.GetKeyDown(KeyCode.C) && previewInstance != null) // Replace with the button you want to use for confirmation
        {
            ConfirmPlacement();
        }

        if (previewInstance != null)
        {
            UpdatePreview();
        }
    }

    void TogglePreviewMode()
    {
        if (previewInstance == null)
        {
            previewInstance = Instantiate(barricadePreview);
            // Set preview properties (e.g., transparency, color)
        }
        else
        {
            Destroy(previewInstance);
            previewInstance = null;
        }
    }

    void UpdatePreview()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Update the preview position based on hit point and normal
            Vector3 previewPosition = hit.point + hit.normal * barricadePrefab.GetComponent<Renderer>().bounds.extents.y;
            previewInstance.transform.position = previewPosition;

            // Use the player's forward direction to determine the rotation
            Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.transform.forward, Vector3.up), Vector3.up);
            previewInstance.transform.rotation = rotation;
        }
        else
        {
            // If no hit, update the preview in a default direction 
            Vector3 previewPosition = ray.origin + ray.direction * 5f;
            previewPosition.y += 5f;
            previewInstance.transform.position = previewPosition;

            previewInstance.transform.rotation = player.transform.rotation;
        }
    }

    void ConfirmPlacement()
    {
        // Perform the actual instantiation of the barricade
        Instantiate(barricadePrefab, previewInstance.transform.position, previewInstance.transform.rotation);

        // Clean up the preview
        Destroy(previewInstance);
        previewInstance = null;

        // Add any additional logic for confirmation (e.g., spend points)
        gameManager.instance.updatePointCount(-500);
    }

    // Add your existing code for point handling and other functionality here
}
