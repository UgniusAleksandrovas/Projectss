using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_JumpBoost : Pickup
{
    [Header("Jump Settings")]
    //[SerializeField] float jumpHeightMultiplier = 2f;
    [SerializeField] int jumpCount = 2;
    [SerializeField] float duration = 2f;
    
    public override void PickupItem(RatController player)
    {
        //player.StartCoroutine(player.ChangeJumpForce(duration, jumpHeightMultiplier));
        player.StartJumpBoost(duration, jumpCount);
    }
}
