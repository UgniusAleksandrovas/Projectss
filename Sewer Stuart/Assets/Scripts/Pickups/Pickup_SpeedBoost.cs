using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_SpeedBoost : Pickup
{
    [Header("Speed Settings")]
    [SerializeField] float speedMultiplier = 2f;
    [SerializeField] float speedDuration = 2f;

    public override void PickupItem(RatController player)
    {
        player.StartCoroutine(player.ChangeSpeed(speedDuration, speedMultiplier));
    }
}
