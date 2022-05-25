using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Inventory inventory;
    public int i;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }
    public void DropItem()
    {
        SpawnItemFromInventory itemDrop = transform.GetComponentInChildren<SpawnItemFromInventory>();
        if (itemDrop != null)
        {
            itemDrop.gameObject.GetComponent<SpawnItemFromInventory>().SpawnDroppedItem();
            inventory.isFull[i] = false;
            Destroy(itemDrop.gameObject);
        }
    }
}
