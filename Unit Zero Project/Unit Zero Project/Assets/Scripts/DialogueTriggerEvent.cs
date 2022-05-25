using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerEvent : DialogueTrigger {
    public override void NextDialogue () {
        dm.DisplayNextSentence();
        if (dm.endText) {
            GetComponent<EventTrigger>().CallOtherFunctions();
            enabled = false;
        }
    }
}
