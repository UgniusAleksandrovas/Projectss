using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    [SerializeField] bool damageOverTime;
    [SerializeField] int damage = 50;
    [SerializeField] float tickTime = 0.1f;
 
    Coroutine DOT;

    void OnTriggerEnter2D(Collider2D collision) {
        IDamageable healthScript = collision.GetComponent<IDamageable>();
        if (healthScript != null) {
            if (damageOverTime) {
                if (DOT == null) {
                    DOT = StartCoroutine(healthScript.DamageOverTime(damage, tickTime, 0f));
                }
            }
            else {
                healthScript.Damage(damage);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        IDamageable healthScript = collision.GetComponent<IDamageable>();
        if (healthScript != null) {
            if (damageOverTime) {
                if (DOT != null) {
                    StopCoroutine(DOT);
                    DOT = null;
                }
            }
        }
    }
}
