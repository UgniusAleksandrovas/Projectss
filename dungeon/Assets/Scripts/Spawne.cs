using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawne : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] SpawnPoint;
    private int rand;
    private int randPosition;
    public float StartTimeBetweenSpawns;
    private float TimeBetweenSpawns = 1f;

    private void Start()
    {
        TimeBetweenSpawns = StartTimeBetweenSpawns;
    }

    private void Update()
    {   
        if(TimeBetweenSpawns <= 0)
        {
            rand = Random.Range(0, enemies.Length);
            randPosition = Random.Range(0, SpawnPoint.Length);
            Instantiate(enemies[rand], SpawnPoint[randPosition].transform.position, Quaternion.identity);
            TimeBetweenSpawns = StartTimeBetweenSpawns;
        }
        else
        {
            TimeBetweenSpawns -= Time.deltaTime;
        }
    }
}


