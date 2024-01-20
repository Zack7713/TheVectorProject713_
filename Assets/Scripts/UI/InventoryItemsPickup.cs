using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemsPickup : MonoBehaviour
{
    [SerializeField] inventoryItems item;
    bool triggerSet;
    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") & !triggerSet)
        {
            triggerSet = true;
            gameManager.instance.playerScript.getInventoryItem(item);
            Destroy(gameObject);
        }
    }
}
