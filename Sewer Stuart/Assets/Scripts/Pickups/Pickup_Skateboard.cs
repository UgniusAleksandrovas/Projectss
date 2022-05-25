using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Skateboard : Pickup
{
    [Header("Skateboard Settings")]
    [SerializeField] float speedMultiplier = 1.5f;
    [SerializeField] float speedDuration = 6f;

    public override void PickupItem(RatController player)
    {
        player.StartSkateboarding(speedDuration, speedMultiplier);
    }
}
