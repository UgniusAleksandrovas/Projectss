using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] bool loadDataOnAwake;
    public int cheeseCount;
    public List<int> purchasedSkins;
    public int activeSkin;
    public List<int> purchasedEmotes;

    public delegate void CheeseCountChanged();
    public static event CheeseCountChanged OnCheeseCountChanged;

    public delegate void ChangeActiveSkin();
    public static event ChangeActiveSkin OnChangeActiveSkin;

    void Awake()
    {
        if (loadDataOnAwake)
        {
            LoadData();
        }
    }

    public void AddCheese(int amount)
    {
        cheeseCount += amount; 
        if (OnCheeseCountChanged != null)
        {
            OnCheeseCountChanged();
        }
        SaveData();
    }

    public void AddSkin(int id)
    {
        purchasedSkins.Add(id);
        EquipSkin(id);
    }

    public void EquipSkin(int id)
    {
        activeSkin = id;
        if (OnChangeActiveSkin != null)
        {
            OnChangeActiveSkin();
        }
        SaveData();
    }

    public void AddEmote(int id)
    {
        purchasedEmotes.Add(id);
        SaveData();
    }

    [ContextMenu("Save Data")]
    public void SaveData()
    {
        SaveSystem.SaveProgress(this);
    }

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        SaveData data = SaveSystem.LoadProgress();

        cheeseCount = data.cheeseCount;

        purchasedSkins = new List<int>();
        for (int i = 0; i < data.purchasedSkins.Length; i++)
        {
            purchasedSkins.Add(data.purchasedSkins[i]);
        }

        activeSkin = data.activeSkin;

        purchasedEmotes = new List<int>();
        for (int i = 0; i < data.purchasedEmotes.Length; i++)
        {
            purchasedEmotes.Add(data.purchasedEmotes[i]);
        }
    }
}
