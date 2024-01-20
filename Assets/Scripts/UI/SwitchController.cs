using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public GameObject door; // The door that this switch controls
    private bool isDoorOpen = false; // Track whether the door is open or closed
    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        // Store the initial position of the door as the closed position
        closedPosition = door.transform.position;

        // Calculate the open position of the door
        float doorLength = door.GetComponent<Renderer>().bounds.size.z;
        openPosition = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z - doorLength + 0.1f);
    }

    public void ActivateSwitch()
    {
        // Toggle the state of the door
        isDoorOpen = !isDoorOpen;

        // Stop any MoveDoor coroutines that are currently running
        StopCoroutine("MoveDoor");

        // Start a new MoveDoor coroutine
        StartCoroutine("MoveDoor");
    }

    IEnumerator MoveDoor()
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime; // This should be adjusted depending on how fast you want the door to move

            // Lerp the door's position
            door.transform.position = Vector3.Lerp(isDoorOpen ? closedPosition : openPosition, isDoorOpen ? openPosition : closedPosition, time);

            yield return null;
        }
    }
}
