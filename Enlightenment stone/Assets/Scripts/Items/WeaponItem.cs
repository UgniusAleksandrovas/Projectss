using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Useitems
{
    [SerializeField] int slotID;
    Inventory inv;
    PlayerScript ps;
    [SerializeField] int damage;

    public override void Initialise()
    {
        ps = FindObjectOfType<PlayerScript>();
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
            ps.damage = damage;

        }
        inv.inventorySlots[i].isFull = false;
    }
}

