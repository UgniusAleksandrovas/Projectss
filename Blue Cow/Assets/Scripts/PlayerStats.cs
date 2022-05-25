using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float moveSpeed;
    public float jumpSpeed;

    PlayerController pc;

    void Awake() {
        pc = GetComponent<PlayerController>();
    }

    public void UpdatePlayerStats() {
        pc.moveSpeed = moveSpeed;
        pc.jumpSpeed = jumpSpeed;
    }
}
