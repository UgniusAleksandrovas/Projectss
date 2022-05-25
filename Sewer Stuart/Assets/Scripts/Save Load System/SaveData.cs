using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int cheeseCount;
    public int[] purchasedSkins;
    public int activeSkin;
    public int[] purchasedEmotes;

    public SaveData(Inventory inventory)
    {
        cheeseCount = inventory.cheeseCount;
        purchasedSkins = inventory.purchasedSkins.ToArray();
        activeSkin = inventory.activeSkin;
        purchasedEmotes = inventory.purchasedEmotes.ToArray();
    }
}
