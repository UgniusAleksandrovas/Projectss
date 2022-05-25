using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPowerUp : MonoBehaviour
{
    [SerializeField] LayerMask collisionLayers;
    public GameObject model;

    float pullDistance;
    float pullStrength;

    RatController player;
    TunnelSpawner tunnelSpawner;

    private void OnEnable()
    {
        model.SetActive(true);
        player = FindObjectOfType<RatController>();
        tunnelSpawner = FindObjectOfType<TunnelSpawner>();
    }

    private void OnDisable()
    {
        model.SetActive(false);
    }
    
    public void StartMagnet(float dist, float strength)
    {
        pullDistance = dist;
        pullStrength = strength;
    }

    private void Update()
    {
        Collider[] pickups = Physics.OverlapSphere(transform.position, pullDistance, collisionLayers);
        for (int i = 0; i < pickups.Length; i++)
        {
            //only pick up cheese (point pickups)
            Pickup_Points pickup = pickups[i].GetComponent<Pickup_Points>();
            if (pickup != null)
            {
                if (pickup.value >= 1)
                {
                    Vector3 dir = transform.position - pickup.transform.position;
                    pickup.transform.localPosition += dir.normalized * (tunnelSpawner.moveSpeed + player.moveSpeed + pullStrength) * Time.deltaTime;
                }
            }
        }
    }
}
