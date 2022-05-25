using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Worm : Obstacle
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.transform.position += new Vector3(0f, 0f, -2f * Time.deltaTime);
    }

    public override void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<RatController>();
        if (player != null)
        {
            hitSound.Play();

            if (playerImmortalityTimer > 0f)
            {
                player.SetIgnoreObstacles(playerImmortalityTimer, playerImmortalityMaterial);
            }
        }

        base.OnTriggerEnter(other);
    }
    public override void OnTriggerExit(Collider other) { }
}
