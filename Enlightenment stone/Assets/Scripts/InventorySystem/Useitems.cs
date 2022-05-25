using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Useitems : MonoBehaviour
{
    private Transform player;
    [HideInInspector] public Inventory inventory;
    public int i;


    private void Start()
    {
        Initialise();
    }

    public virtual void Initialise()
    {
        player = FindObjectOfType<PlayerScript>().transform;
        inventory = FindObjectOfType<Inventory>();
    }

    public virtual void Use()
    {
        inventory.inventorySlots[i].isFull = false;
        Destroy(gameObject);
    }
}
