using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_NightVisionGoggles : Pickup
{
    [Header("Night Vision Settings")]
    [SerializeField] float nightVisionDuration = 5f;

    public override void PickupItem(RatController player)
    {
        //player.AddNightVisiontime(nightVisionDuration);
        player.EnableNightVision(true);
    }
}
