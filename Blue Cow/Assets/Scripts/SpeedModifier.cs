using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : MonoBehaviour {

    public float speedMultiplier = 1f;

    void OnCollisionEnter2D(Collision2D collision) {
        PlayerStats ps = collision.gameObject.GetComponent<PlayerStats>();
        if (ps != null) {
            ps.moveSpeed *= speedMultiplier;
            ps.UpdatePlayerStats();
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        PlayerStats ps = collision.gameObject.GetComponent<PlayerStats>();
        if (ps != null) {
            ps.moveSpeed /= speedMultiplier;
            ps.UpdatePlayerStats();
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        PlayerStats ps = collision.gameObject.GetComponent<PlayerStats>();
        if (ps != null) {
            ps.moveSpeed *= speedMultiplier;
            ps.UpdatePlayerStats();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        PlayerStats ps = collision.gameObject.GetComponent<PlayerStats>();
        if (ps != null) {
            ps.moveSpeed /= speedMultiplier;
            ps.UpdatePlayerStats();
        }
    }
}
