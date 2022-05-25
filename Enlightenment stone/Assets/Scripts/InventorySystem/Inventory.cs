using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    //head in slot 12
    //chest in slot 13
    //legs in slot 14
    //feet in slot 15

    //weapon in slot 16
}
[System.Serializable]
public class InventorySlot
{
    public bool isFull;
    public GameObject slot;
}