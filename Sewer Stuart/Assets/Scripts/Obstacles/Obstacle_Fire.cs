using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Fire : Obstacle
{
    [Header("Fire Settings")]
    [SerializeField] float screenFlamesCount = 100f;
    [SerializeField] float burnDuration = 2f;

    public override void OnCollisionEnter(Collision other) { }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (player != null)
        {
            //player.SetFlamesCount(screenFlamesCount);
            player.SetBurnDuration(burnDuration);
        }
    }

    public override void OnTriggerExit(Collider other) { }
}
