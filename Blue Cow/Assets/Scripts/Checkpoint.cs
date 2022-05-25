using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public Transform spawnPos;/*
    public GameObject headsUpUI;
    public GameObject saveUI;

    bool nearCheckpoint;*/
    CheckpointManager theCM;

    // Start is called before the first frame update
    void Start() {
        theCM = FindObjectOfType<CheckpointManager>();
    }
    /*
    private void Update() {
        if (nearCheckpoint) {
            if (Input.GetKeyDown(KeyCode.E)) {
                headsUpUI.SetActive(false);
                saveUI.SetActive(true);
                nearCheckpoint = false;
            }
        }
    }
    */
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<PlayerController>()) {
            theCM.lastCheckpoint = spawnPos.transform;/*
            nearCheckpoint = true;
            headsUpUI.SetActive(true);
            saveUI.SetActive(false);*/
        }
    }
    /*
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<PlayerController>()) {
            nearCheckpoint = false;
            headsUpUI.SetActive(false);
            saveUI.SetActive(false);
        }
    }*/
}
