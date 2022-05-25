using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemFromInventory : MonoBehaviour
{
    public GameObject item;
    private PlayerScript player;


    private void Awake()
    {
       player = FindObjectOfType<PlayerScript>();
    }
    public void SpawnDroppedItem()
    {
        Vector3 playerPos = player.transform.position;
        GameObject itemSpawn = Instantiate(item, playerPos, Quaternion.identity);
        print(player.transform.position + " --- " + playerPos);
        itemSpawn.GetComponent<Pickups>().canPickUp = false;
        FindObjectOfType<audioManager>().Play("PickingItem");
    }
}
