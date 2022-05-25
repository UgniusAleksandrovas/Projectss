using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour {

    public GameObject noteStatic;
    public GameObject notePreview;
    
    private GameObject headsUpUI;
    private bool nearItem;
    private bool activated;

    private FPSController thePlayer;

    private void Start() {
        thePlayer = FindObjectOfType<FPSController>();
        StopPreview();
    }

    private void Update() {
        if (nearItem) {
            if (Input.GetKeyDown(KeyCode.E)) {
                if (activated == true) {
                    StopPreview();
                }
                else {
                    headsUpUI.SetActive(false);
                    activated = true;
                    PreviewNote();
                }
            }
        }
    }

    private void OnTriggerStay(Collider collision) {
        if (collision.GetComponent<FPSController>()) {
            headsUpUI = GameObject.Find("Canvas").transform.Find("Interact HUD").gameObject;
            if (activated == false) headsUpUI.SetActive(true);
            nearItem = true;
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.GetComponent<FPSController>()) {
            headsUpUI.SetActive(false);
            activated = false;
            nearItem = false;
            StopPreview();
        }
    }

    private void PreviewNote() {
        notePreview.SetActive(true);
        noteStatic.SetActive(false);
        thePlayer.freezeRotation = true;
        thePlayer.freezePosition = true;
    }

    private void StopPreview() {
        noteStatic.SetActive(true);
        notePreview.SetActive(false);
        thePlayer.freezeRotation = false;
        thePlayer.freezePosition = false;
    }
}
