using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadePickUp : MonoBehaviour {
    
    public Sprite weaponImage;

    private GameObject headsUpUI;
    private bool nearItem;
    private GrenadeManager thePlayerInv;

    private void Update() {
        if (nearItem) {
            if (Input.GetKeyDown(KeyCode.E)) {
                if (thePlayerInv.grenadeCount < thePlayerInv.grenadeIcons.Length) {
                    thePlayerInv.grenadeCount++;
                    thePlayerInv.UpdateSlots();
                    headsUpUI.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider collision) {
        if (collision.GetComponent<FPSController>()) {
            thePlayerInv = collision.GetComponent<GrenadeManager>();
            headsUpUI = GameObject.Find("Canvas").transform.Find("Pickup HUD").gameObject;
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
