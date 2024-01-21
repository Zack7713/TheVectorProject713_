using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InventoryItemsData : MonoBehaviour
{
    public inventoryItems item;
    [Range(0f, 1f)]
    public float dropChance;
    public int remainingCount;
}
