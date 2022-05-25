using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour {

    public int health;
    public int maxHealth;
    public bool regeneration;
    public int regenRate;
    [HideInInspector] public float regenDelay;
    public float initialRegenDelay;
    private float regenTimer;

    private void Update() {
        if (regeneration) {
            if (regenDelay >= 0) regenDelay -= Time.deltaTime;
            if (regenDelay <= 0) Regenerate();
        }
    }

    public virtual void UpdateHealth() {
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    public void Regenerate() {
        if (regenTimer <= 0) {
            if (health < maxHealth) {
                health += 1;
                UpdateHealth();
            }
            else {
                regenDelay = initialRegenDelay;
            }
            regenTimer = 1;
        }
        regenTimer -= regenRate * Time.deltaTime;
    }

    public IEnumerator DamageOverTime(float totalAmount, float amount, float interval) {
        float endHealth = health - totalAmount;
        while (health > endHealth) {
            yield return new WaitForSeconds(interval);
            health -= Mathf.RoundToInt(amount);
            UpdateHealth();
        }
    }
}
