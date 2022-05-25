using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Inventory inventory;
    public int i;
    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }
    public void DropItem()
    {
        SpawnItemFromInventory itemDrop = transform.GetComponentInChildren<SpawnItemFromInventory>();
        if (itemDrop != null)
        {
            itemDrop.gameObject.GetComponent<SpawnItemFromInventory>().SpawnDroppedItem();
            inventory.inventorySlots[i].isFull = false;
            Destroy(itemDrop.gameObject);
        }
    }
}
