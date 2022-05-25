using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ShopItem[] skins;
    [SerializeField] ShopItem[] emotes;
    [SerializeField] Inventory inventory;

    void Start()
    {
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].UpdateCost();
        }
        UpdateItemsAvailability();
    }

    public void UpdateItemsAvailability()
    {
        //hide purchase button for skins
        for (int i = 0; i < inventory.purchasedSkins.Count; i++)
        {
            skins[inventory.purchasedSkins[i]].Purchased();
        }

        //hide equipped skin button
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].Equip(false);
            if (i == inventory.activeSkin)
            {
                skins[i].Equip(true);
            }
        }

        //hide purchase button for emotes
        for (int i = 0; i < inventory.purchasedEmotes.Count; i++)
        {
            emotes[inventory.purchasedEmotes[i]].Purchased();
        }
    }

    public void PurchaseSkin(ShopItem item)
    {
        if (inventory.cheeseCount >= item.cost)
        {
            inventory.AddCheese(-item.cost);
            inventory.AddSkin(item.ID);
            UpdateItemsAvailability();
        }
        else
        {
            item.TransactionFailed();
        }
    }

    public void EquipSkin(ShopItem item)
    {
        inventory.EquipSkin(item.ID);
        UpdateItemsAvailability();
    }

    public void PurchaseEmote(ShopItem item)
    {
        if (inventory.cheeseCount >= item.cost)
        {
            inventory.AddCheese(-item.cost);
            inventory.AddEmote(item.ID);
            UpdateItemsAvailability();
        }
        else
        {
            item.TransactionFailed();
        }
    }
}
