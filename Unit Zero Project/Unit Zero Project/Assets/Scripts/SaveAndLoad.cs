using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour {

    public PlayerHealth playerHP;
    public Inventory inv;
    public GrenadeManager grenadeInv;
    public bool loadOnStart;

    private void Start() {
        if (loadOnStart) LoadPlayer();
    }

    public void SavePlayer() {
        SaveSystem.SavePlayer(playerHP, inv, grenadeInv);
    }

    public void LoadPlayer() {
        PlayerData data = SaveSystem.LoadPlayer();
        playerHP.health = data.health;
        for (int i = 0; i < data.weapons.Length; i++) {
            if (data.weapons[i] != "") {
                GameObject gun = Instantiate(Resources.Load("Weapons/" + data.weapons[i], typeof(GameObject))) as GameObject;
                inv.weapons[i] = gun;
                gun.GetComponent<Gun>().inventorySlot = i;
                inv.weapons[i].GetComponent<Gun>().MagAmmo = data.magAmmos[i];
                inv.weapons[i].GetComponent<Gun>().Ammo = data.ammos[i];
                inv.selectedSlot = data.selectedWeapon;
                //inv.WeaponSelect(inv.weapons);
                //inv.UpdateInventory();
                //inv.UpdateWeaponInfo();
            }
        }
        StartCoroutine(WaitToReloadInventory());
        grenadeInv.grenadeCount = data.grenadeCount;
        grenadeInv.UpdateSlots();
    }

    private IEnumerator WaitToReloadInventory() {
        yield return null;
        inv.WeaponSelect(inv.weapons);
    }
}
