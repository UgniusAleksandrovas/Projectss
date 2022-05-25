using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] int minNumber = 0;
    [SerializeField] int maxNumber = 10;
    [SerializeField] Vector3 tunnelSize = new Vector3(5f, 5f, 20f);
    [SerializeField] PrefabArray[] pickups;
    float[] prob;

    void Awake() 
    {
        prob = new float[pickups.Length];
        for (int i = 0; i < pickups.Length; i++) 
        {
            prob[i] = pickups[i].rarity;
        }
    }

    void Start() 
    {
        int rand = Random.Range(minNumber, maxNumber);
        for (int i = 0; i < rand; i++)
        {
            int randomizer = Choose(prob);

            float zPos = Random.Range(-tunnelSize.z / 2f, tunnelSize.z / 2f);
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

            Vector3 spawnPos = (dir * (tunnelSize.x - pickups[randomizer].zOffset)) + (Vector3.forward * zPos);
            float angle = Vector2.SignedAngle(Vector3.up, -dir);
            Quaternion spawnRot = Quaternion.AngleAxis(angle, Vector3.forward);

            if (!pickups[randomizer].ignoreCollisionOnSpawn)
            {
                int defaultLayer = 0;
                int obstacleLayer = 7;
                int mask = ((1 << defaultLayer) | (1 << obstacleLayer));
                Collider[] hitColliders = Physics.OverlapSphere(spawnPos, pickups[randomizer].collisionCheckRadius, mask);
                if (hitColliders.Length > 0)
                {
                    continue;
                }
            }

            GameObject pickup = Instantiate(pickups[randomizer].prefab, transform.position + spawnPos, spawnRot);
            pickup.transform.parent = transform;
            pickup.transform.Rotate(Vector3.up * Random.Range(pickups[randomizer].yawRotation.x, pickups[randomizer].yawRotation.y), Space.Self);
        }
    }

    int Choose(float[] probs) 
    {
        float total = 0;

        foreach (float elem in probs) 
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++) 
        {
            if (randomPoint < probs[i]) 
            {
                return i;
            }
            else 
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
