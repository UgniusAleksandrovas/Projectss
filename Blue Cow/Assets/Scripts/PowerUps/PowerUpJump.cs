using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpJump : PowerUp {

    [SerializeField] float jumpMultiplier = 3f;

    public override void PowerUpStat(PlayerStats stats) {
        stats.jumpSpeed *= jumpMultiplier;
        stats.UpdatePlayerStats();
    }

    public override void PowerDownStat(PlayerStats stats) {
        stats.jumpSpeed /= jumpMultiplier;
        stats.UpdatePlayerStats();
    }
}
