using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;

    private GameObject headsUpUI;
    private bool nearItem;
    private bool activated;

    [HideInInspector] public DialogueManager dm;

    private void Start() {
        dm = FindObjectOfType<DialogueManager>();
    }

    public void TriggerDialogue() {
		dm.StartDialogue(dialogue);
	}

    public virtual void NextDialogue() {
        dm.DisplayNextSentence();
    }

    public void ExitDialogue() {
        dm.EndDialogue();
    }

    private void Update() {
        if (nearItem) {
            if (Input.GetKeyDown(KeyCode.E)) {
                if (activated == true) {
                    NextDialogue();
                }
                else {
                    headsUpUI.SetActive(false);
                    activated = true;
                    TriggerDialogue();
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
            ExitDialogue();
        }
    }
}