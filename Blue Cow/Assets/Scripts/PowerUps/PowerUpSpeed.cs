using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : PowerUp {

    [SerializeField] float speedMultiplier = 2f;

    public override void PowerUpStat(PlayerStats stats) {
        stats.moveSpeed *= speedMultiplier;
        stats.UpdatePlayerStats();
    }

    public override void PowerDownStat(PlayerStats stats) {
        stats.moveSpeed /= speedMultiplier;
        stats.UpdatePlayerStats();
    }
}
