using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public int damage = 20;
    void OnCollisionEnter(Collision collision)
    {
        IDamageable Player = collision.gameObject.GetComponent<IDamageable>();

        if (Player != null)
        {
            Player.TakeDamage(-damage);
        }
    }
}
