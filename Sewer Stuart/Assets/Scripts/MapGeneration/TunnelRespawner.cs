using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelRespawner : MonoBehaviour
{
    public Transform spawnPoint;
    
    float moveSpeed;

    TunnelSpawner tunnelSpawner;

    void Start()
    {
        tunnelSpawner = FindObjectOfType<TunnelSpawner>();
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.fixedDeltaTime);
    }

    //When Exiting a tunnel, destroys it, Spawns another one in the front.
    private void OnTriggerExit(Collider other) 
    {
        RatController player = other.GetComponent<RatController>();
        if (player != null) 
        {
            tunnelSpawner.SpawnNextTunnel();
            Destroy(gameObject, 2f);
        }
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}