using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorBarController : MonoBehaviour
{

    public Image ArmorBar;
    public float armor;
    public float MaxArmor;

    private void Start()
    {
        UpdateArmorBarUI();
    }

    public void TakeArmor(int armour)
    {
        armor -= armour;

        armor = Mathf.Clamp(armor, 0, MaxArmor);
        UpdateArmorBarUI();
    }


    void UpdateArmorBarUI()
    {
        ArmorBar.fillAmount = armor / MaxArmor;
    }
}
