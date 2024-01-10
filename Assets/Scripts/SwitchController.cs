using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public List<GameObject> objectsToActivate = new List<GameObject>();


    public void ActivateSwitch()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null)
            {
                Debug.Log("Collider info: " + collider.bounds.ToString());
            }

            // Implement activation logic here
            // For now, just toggle the GameObjects on and off
            obj.SetActive(!obj.activeSelf);
        }
    }
}
