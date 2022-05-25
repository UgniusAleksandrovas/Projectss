using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourItem : Useitems
{
    [SerializeField] int slotID;
    Inventory inv;


    private ArmorBarController armorBar;

    [SerializeField] int armor;


    private void Awake()
    {
        armorBar = FindObjectOfType<ArmorBarController>();
    }

    public override void Initialise()
    {
        inv = FindObjectOfType<Inventory>();
        base.Initialise();
    }

    public override void Use()
    {
        if (!inv.inventorySlots[slotID].isFull)
        {
            FindObjectOfType<audioManager>().Play("wieldArmour");
            transform.parent = inv.inventorySlots[slotID].slot.transform;
            transform.localPosition = Vector3.zero;
            armorBar.TakeArmor(-armor);
            Destroy(gameObject);
        }
        inv.inventorySlots[i].isFull = false;
    }
}
