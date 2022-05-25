using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Magnet : Pickup
{
    [Header("Magnet Settings")]
    [SerializeField] float pullDistance = 5f;
    [SerializeField] float pullStrength = 3f;
    [SerializeField] float duration = 5f;

    public override void PickupItem(RatController player)
    {
        player.EnableMagnet(duration, pullDistance, pullStrength);
    }
}
