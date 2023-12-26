using UnityEngine;
using System.Collections.Generic;

public class doorOpener : MonoBehaviour
{
    // List of doors associated with this button
    public List<SlidingDoorScript> doors = new List<SlidingDoorScript>();

    // Define the key to open the doors
    public KeyCode openKey = KeyCode.O;

    // Update is called once per frame
    void Update()
    {
        // Check if the specified key is pressed
        if (Input.GetKeyDown(openKey))
        {
            // Iterate through the list of doors and open/close each one
            foreach (var door in doors)
            {
                // Check if the door reference is not null
                if (door != null)
                {
                    // Call the ToggleDoor method in the door script
                    door.ToggleDoor();
                }
                else
                {
                    Debug.LogError("Door reference is not set in the KeyDoorOpener script!");
                }
            }
        }
    }
}
