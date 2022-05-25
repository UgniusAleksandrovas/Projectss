using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {

    public bool immortal;
    public int health = 100;
    public int maxHealth = 100;

    [Header("Visuals")]
    [SerializeField] bool renderHitVisuals;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Color hitColor;
    [SerializeField] float colorTimer;
    [SerializeField] AudioSource soundEffect;

    public virtual void Initialise() {
        UpdateHealth();
    }

    void Start() {
        Initialise();
    }

    public void Damage(int damage) {
        if (!immortal) {
            health -= damage;
            if(soundEffect != null)
            {
                soundEffect.Play();
            }
            StartCoroutine(HitVisuals(colorTimer, damage));
            UpdateHealth();
        }
    }

    public IEnumerator DamageOverTime(int damage, float tickTime, float duration) {
        if (duration == 0) {
            while (true) {
                yield return new WaitForSeconds(tickTime);
                Damage(damage);
                duration -= tickTime;
            }
        }
        else {
            while (duration > 0) {
                yield return new WaitForSeconds(tickTime);
                Damage(damage);
                duration -= tickTime;
            }
            yield break;
        }
    }

    public void UpdateHealth() {
        if (GetComponent<HealthUI>()) {
            GetComponent<HealthUI>().UpdateUI();
        }
        if (health <= 0) {
            DestroyObject();
        }
    }

    public virtual void DestroyObject() {
        health = 0;
        Destroy(gameObject);
    }

    IEnumerator HitVisuals(float timer, int damage) {
        if (renderHitVisuals) {
            if (damage > 0) {
                sr.color = hitColor;
                yield return new WaitForSeconds(timer);
                sr.color = Color.white;
                yield break;
            }
        }
    }
}
