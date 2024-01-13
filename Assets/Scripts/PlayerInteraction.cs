using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float raycastDistance = 5f;

    public cameraControls camController;
    [SerializeField] GameObject menuInteract;
    void Start()
    {
        camController = Camera.main.GetComponentInParent<cameraControls>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Default"); // Adjust the layer name as needed

            Vector3 raycastOrigin = camController.transform.position;
            Vector3 raycastDirection = camController.transform.forward;

            Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.red, 1f); // Visualize the raycast

            if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance, layerMask))
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                SwitchController switchController = hit.collider.GetComponent<SwitchController>();
                if (switchController != null)
                {
                    Debug.Log("Switch found and activated!");
                    // Activate the switch
                    switchController.ActivateSwitch();
                }
                else
                {
                    Debug.Log("Switch not found!");
                }
            }
            else
            {
                Debug.Log("Raycast didn't hit anything.");
            }
        }
    }
}
