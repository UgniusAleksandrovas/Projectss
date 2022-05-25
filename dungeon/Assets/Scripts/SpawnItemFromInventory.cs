using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemFromInventory : MonoBehaviour
{
    public GameObject item;
    private Transform player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void SpawnDroppedItem()
    {
        Vector2 playerPos = new Vector2(player.position.x, player.position.y);
        GameObject itemSpawn = Instantiate(item, playerPos, Quaternion.identity);
        itemSpawn.GetComponent<Pickups>().canPickUp = false;
    }
}
