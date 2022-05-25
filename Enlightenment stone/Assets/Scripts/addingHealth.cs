using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addingHealth : MonoBehaviour
{
    float lastActivity = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        lastActivity = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f);

        foreach (var col in colliders)
        {
            IDamageable healthItem = col.GetComponent<IDamageable>();

            if (healthItem != null && Time.time > lastActivity + 1f)
            {
                healthItem.TakeDamage(-30);
                lastActivity = Time.time;
                GameObject.Destroy(gameObject);
            }
        }
    }
}
