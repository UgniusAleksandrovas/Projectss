using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Points : Pickup
{
    [Header("Cheese Settings")]
    public int value = 1;

    PlayerScore playerScore;

    public override void Initialize()
    {
        playerScore = FindObjectOfType<PlayerScore>();
    }

    public override void PickupItem(RatController player)
    {
        playerScore.AddScore(value);
    }
}
