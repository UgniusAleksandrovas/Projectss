using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health {

    CheckpointManager cm;


    public override void Initialise() {
        cm = FindObjectOfType<CheckpointManager>();
        base.Initialise();
    }

    public override void DestroyObject() {
        health = 0;
        cm.RespawnPlayer();
    }
}
