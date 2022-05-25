using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHealth : PowerUp {

    [SerializeField] int healthBonus = 50;

    public override void PowerUpStat(PlayerStats stats) {
        Health playerHealth = stats.GetComponent<Health>();
        playerHealth.health += healthBonus;
        playerHealth.UpdateHealth();
    }

    public override void PowerDownStat(PlayerStats stats) {
        Health playerHealth = stats.GetComponent<Health>();
        if (playerHealth.health > playerHealth.maxHealth) {
            playerHealth.health -= playerHealth.health - playerHealth.maxHealth;
        }
        stats.UpdatePlayerStats();
        playerHealth.UpdateHealth();
    }
}
