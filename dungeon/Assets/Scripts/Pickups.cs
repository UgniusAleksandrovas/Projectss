using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;

    public bool canPickUp = true;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canPickUp)
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                for (int i = 0; i < inventory.slots.Length; i++)
                {
                    if (inventory.isFull[i] == false)
                    {
                        inventory.isFull[i] = true;
                        GameObject item = Instantiate(itemButton, inventory.slots[i].transform, false);
                        if (item.GetComponent<Useitems>() != null) {
                            item.GetComponent<Useitems>().i = i;
                        }
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            canPickUp = true;
        }
    }

}

