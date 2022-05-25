using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Useitems : MonoBehaviour
{
    public GameObject effect;
    private Transform player;
    //public HealthBar healthBar;
    public int healAmount;
    private Health health;
    private Inventory inventory;
    public int i;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        inventory = FindObjectOfType<Inventory>();
        health = player.GetComponent<Health>();
    }
    public void Use()
    {
        Destroy(gameObject);
        Instantiate(effect, player.position, Quaternion.identity);
        /*HealthSystem healthSystem = new HealthSystem(100);
        healthBar.Setup(healthSystem);
        healthSystem.Damage(10);
        Debug.Log("Health: " + healthSystem.GetHealthPercent());*/
        health.Heal(healAmount);
        inventory.isFull[i] = false;
    }
}
