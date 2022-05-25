using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {

    public int health;
    public string[] weapons;
    public int[] magAmmos;
    public int[] ammos;
    public int selectedWeapon;
    public int grenadeCount;

    public PlayerData (PlayerHealth playerHP, Inventory inv, GrenadeManager grenadeInv) {
        health = playerHP.health;
        weapons = new string[inv.weapons.Count];
        magAmmos = new int[inv.weapons.Count];
        ammos = new int[inv.weapons.Count];
        for (int i = 0; i < weapons.Length; i++) {
            if (inv.weapons[i] == null) {
                weapons[i] = "";
                magAmmos[i] = 0;
                ammos[i] = 0;
                continue;
            }
            weapons[i] = inv.weapons[i].GetComponent<Gun>().name.Replace("(Clone)", "");
            magAmmos[i] = inv.weapons[i].GetComponent<Gun>().MagAmmo;
            ammos[i] = inv.weapons[i].GetComponent<Gun>().Ammo;
        }
        selectedWeapon = inv.selectedSlot;
        grenadeCount = grenadeInv.grenadeCount;
    }

}
