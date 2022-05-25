using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Immortal : Pickup
{
    [Header("Immortal Settings")]
    [SerializeField] float duration = 5f;
    [SerializeField] Material playerMaterial;

    public override void PickupItem(RatController player)
    {
        player.SetIgnoreObstacles(duration, playerMaterial);
    }
}
