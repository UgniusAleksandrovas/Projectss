using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSuper : PowerUp {

    [SerializeField] int healthBonus = 50;
    [SerializeField] float speedMultiplier = 2f;
    [SerializeField] float jumpMultiplier = 3f;

    public override void PowerUpStat(PlayerStats stats) {
        Health playerHealth = stats.GetComponent<Health>();
        playerHealth.health += healthBonus;
        stats.moveSpeed *= speedMultiplier;
        stats.jumpSpeed *= jumpMultiplier;
        playerHealth.immortal = true;
        playerHealth.UpdateHealth();
        stats.UpdatePlayerStats();
    }

    public override void PowerDownStat(PlayerStats stats) {
        Health playerHealth = stats.GetComponent<Health>();
        playerHealth.health -= playerHealth.health - playerHealth.maxHealth;
        stats.moveSpeed /= speedMultiplier;
        stats.jumpSpeed /= jumpMultiplier;
        playerHealth.immortal = false;
        playerHealth.UpdateHealth();
        stats.UpdatePlayerStats();
    }
}
