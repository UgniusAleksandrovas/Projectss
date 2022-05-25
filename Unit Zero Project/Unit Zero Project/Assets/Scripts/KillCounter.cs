using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCounter : MonoBehaviour {

    public int kills;
    public int requiredKillsForDialogue;
    public GameObject dialogue;
    public GameObject barrier;

    public void UpdateKills() {
        if (kills == requiredKillsForDialogue) {
            StartDialogue();
        }
    }

    public void StartDialogue() {
        dialogue.SetActive(true);
        barrier.SetActive(false);
    }
}
