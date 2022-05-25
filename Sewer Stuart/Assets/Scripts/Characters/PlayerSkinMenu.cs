using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinMenu : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    [Header("Skin Settings")]
    [SerializeField] PlayerHop playerScript;
    int activeSkin = 0;
    [SerializeField] SkinMenu[] allSkins;

    void OnEnable()
    {
        Inventory.OnChangeActiveSkin += SetActiveSkin;
    }

    void OnDisable()
    {
        Inventory.OnChangeActiveSkin -= SetActiveSkin;
    }

    void Start()
    {
        SetActiveSkin();
    }

    public void SetActiveSkin()
    {
        activeSkin = inventory.activeSkin;
        for (int i = 0; i < allSkins.Length; i++)
        {
            allSkins[i].skin.SetActive(false);
        }
        allSkins[activeSkin].skin.SetActive(true);
        playerScript.anim = allSkins[activeSkin].anim;
    }
}

[System.Serializable]
public struct SkinMenu 
{
    public string name;
    public GameObject skin;
    public Animator anim;
}
