using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    [HideInInspector] public Transform lastCheckpoint;
    PlayerController pc;
    PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start() {
        pc = FindObjectOfType<PlayerController>();
        playerHealth = pc.GetComponent<PlayerHealth>();
        lastCheckpoint = GameObject.FindWithTag("Spawnpoint").transform;
        RespawnPlayer();
    }

    public void RespawnPlayer() {
        pc.transform.position = lastCheckpoint.position;
        pc.gameObject.SetActive(true);
        playerHealth.health = playerHealth.maxHealth;
        playerHealth.UpdateHealth();
    }
}
