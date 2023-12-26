using UnityEngine;

public class SlidingDoorScript : MonoBehaviour
{
    // The positions for the door in the closed and open states
    public Vector3 closedPosition = Vector3.zero;
    public Vector3 openPosition = new Vector3(5, 0, 0);

    // The speed of the door movement
    public float doorSpeed = 2.0f;

    // The state of the door (true if open, false if closed)
    private bool isOpen = false;

    // Method to open or close the door
    public void ToggleDoor()
    {
        isOpen = !isOpen;

        // Determine the target position based on the door state
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;

        // Smoothly interpolate between the current position and the target position
        StartCoroutine(MoveDoor(transform.position, targetPosition, doorSpeed));
    }

    // Coroutine to smoothly move the door
    private System.Collections.IEnumerator MoveDoor(Vector3 startPosition, Vector3 endPosition, float speed)
    {
        float moveDoor = 0.0f;

        while (moveDoor < 1.0f)
        {
            moveDoor += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPosition, endPosition, moveDoor);
            yield return null;
        }
    }
}
