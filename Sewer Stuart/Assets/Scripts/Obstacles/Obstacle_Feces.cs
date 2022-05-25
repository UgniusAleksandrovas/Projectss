using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Feces : Obstacle
{
    [Header("Feces Settings")]
    [SerializeField] float amount = 10f;
    [SerializeField] float duration = 3f;
    [SerializeField] Vector2 randomScale = new Vector2(0.75f, 1.25f);

    public override void Initialization()
    {
        transform.localScale *= Random.Range(randomScale.x, randomScale.y);
    }

    public override void OnCollisionEnter(Collision other) { }

    public override void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<RatController>();
        if (player != null)
        {
            player.FecesSplat(duration, amount);
        }
        base.OnTriggerEnter(other);
    }
}
