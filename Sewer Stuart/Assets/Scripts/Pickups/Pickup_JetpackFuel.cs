using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_JetpackFuel : Pickup
{
    [Header("Fuel Settings")]
    [SerializeField] int fuelAmount = 50;

    public override void PickupItem(RatController player)
    {
        Jetpack jetpack = player.GetComponent<Jetpack>();
        if (jetpack != null)
        {
            jetpack.AddFuel(fuelAmount);
        }
    }
}
