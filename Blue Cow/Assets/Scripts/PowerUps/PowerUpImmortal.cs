using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpImmortal : PowerUp {

    public override void PowerUpStat(PlayerStats stats) {
        Health playerHealth = stats.GetComponent<Health>();
        playerHealth.immortal = true;
        playerHealth.UpdateHealth();
    }

    public override void PowerDownStat(PlayerStats stats) {
        Health playerHealth = stats.GetComponent<Health>();
        playerHealth.immortal = false;
        playerHealth.UpdateHealth();
    }
}
