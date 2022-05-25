using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFountain : MonoBehaviour
{

    float lastActivity = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        lastActivity = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        IDamageable health = other.GetComponent<IDamageable>();

        if (health != null && Time.time > lastActivity + 0.1f)
        {
            health.TakeDamage(-1);
            lastActivity = Time.time;
        }
    }
}
