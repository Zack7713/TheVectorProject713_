using UnityEngine;

public class doorControlTrigger : MonoBehaviour
{
    [SerializeField] private Animator door;
    [SerializeField] private bool doorOpen;

    bool playerInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.Play("DoorOpen", 0, 0.0f);
            doorOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.Play("DoorClose", 0, 0.0f);
            doorOpen = false;
        }
    }
}
