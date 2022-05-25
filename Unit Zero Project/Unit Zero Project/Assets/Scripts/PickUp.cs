using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour {
    
    public string weaponName;
    public Sprite weaponImage;
    public GameObject model;
    
    private GameObject headsUpUI;
    private bool nearItem;
    private Inventory thePlayerInv;

    private void Update() {
        if (nearItem) {
            if (Input.GetKeyDown(KeyCode.E)) {
                int emptySlot = -1;
                for (int i = 0; i < thePlayerInv.weapons.Count; i++) {
                    if (thePlayerInv.weapons[i] == null) {
                        emptySlot = i;
                        break;
                    }
                }
                if (emptySlot != -1) {
                    GameObject gun = Instantiate(Resources.Load("Weapons/" + weaponName, typeof(GameObject))) as GameObject;
                    gun.GetComponent<Gun>().inventorySlot = emptySlot;
                    thePlayerInv.weapons[emptySlot] = gun;
                    /*thePlayerInv.weaponItems[emptySlot] = thePlayerInv.weapons[emptySlot].GetComponent<Gun>().inventoryItem;
                    GameObject weaponInv = Instantiate(model);
                    weaponInv.transform.parent = thePlayerInv.slots[emptySlot].transform;
                    weaponInv.transform.localPosition = Vector3.zero;
                    weaponInv.transform.localEulerAngles = Vector3.zero;*/
                    headsUpUI.SetActive(false);
                    thePlayerInv.selectedSlot = emptySlot;
                    thePlayerInv.WeaponSelect(thePlayerInv.weapons);
                    Destroy(gameObject);
                }/*
                else {
                    thePlayerInv.selectedSlot = -1;
                    thePlayerInv.newWeapon = Resources.Load("Weapons/" + weaponName, typeof(GameObject)) as GameObject;
                    headsUpUI.SetActive(false);
                    thePlayerInv.showInventory = true;
                    thePlayerInv.replaceWeapon = true;
                    Destroy(gameObject);
                }*/
            }
        }
    }

    private void OnTriggerStay(Collider collision) {
        if (collision.GetComponent<FPSController>()) {
            thePlayerInv = collision.GetComponent<Inventory>();
            headsUpUI = GameObject.Find("Canvas").transform.Find("Pickup HUD").gameObject;
            //headsUpUI.transform.Find("Weapon Name").GetComponent<Text>().text = weaponName;
            headsUpUI.transform.Find("Weapon Sprite").GetComponent<Image>().sprite = weaponImage;
            headsUpUI.SetActive(true);
            nearItem = true;
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.GetComponent<FPSController>()) {
            headsUpUI.SetActive(false);
            nearItem = false;
        }
    }
}
