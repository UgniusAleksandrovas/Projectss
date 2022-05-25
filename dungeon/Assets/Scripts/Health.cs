using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public int health;
    private int healthMax;
    public Image healthBarUI;


    void Start() 
    {
        healthMax = health;   
    }

    public int GetHealth() {
        return health;
    }
    public float GetHealthPercent() {
        return (float)health / healthMax;
    }
    public void Damage(int damageAmount) {
        health -= damageAmount;
        if (health < 0) health = 0;
        UpdateHealth();
    }
    public void Heal(int healAmount) {
        health += healAmount;
        if (health > healthMax) health = healthMax;
        UpdateHealth();
    }

    public virtual void UpdateHealth() {
        healthBarUI.fillAmount = GetHealthPercent();
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
