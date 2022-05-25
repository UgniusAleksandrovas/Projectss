using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_LiquidNitrogen : Obstacle
{
    [Header("Liquid Nitrogen Settings")]
    [SerializeField] float frostIncreaseSpeed = 2f;

    public override void OnCollisionEnter(Collision other) { }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (player != null)
        {
            player.IncraeseFrostScale(0.05f);
        }
    }

    void OnTriggerStay(Collider other)
    {
        player = other.GetComponent<RatController>();
        if (player != null)
        {
            player.IncraeseFrostScale(frostIncreaseSpeed * Time.deltaTime);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
