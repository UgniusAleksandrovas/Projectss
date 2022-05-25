using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelSpawner : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float maxSpeed = 30f;
    public float moveSpeed = 6f;
    [SerializeField] float speedIncrease = 0.05f;

    [Header("Tunnel Settings")]
    [SerializeField] int maxNumberOfTunnels = 10; //max number of tunnels at a time
    [SerializeField] int startNumberOfTunnels = 5; //max number of tunnels at the beginning of the game
    [SerializeField] GameObject emptyTunnel;
    //[SerializeField] PrefabArray[] tunnelTypes; //the rarity of each tunnel
    [SerializeField] TunnelSection[] tunnelSections;
    int currentSection = 0;
    int nextSection = 0;

    //float[] prob;
    int randomizer;

    Transform nextSpawnPoint;
    Vector3 direction;

    RatController player;
    
    void Awake()
    {
        /*
        prob = new float[tunnelTypes.Length];
        for (int i = 0; i < tunnelTypes.Length; i++)
        {
            prob[i] = tunnelTypes[i].rarity;
        }*/
        player = FindObjectOfType<RatController>();
    }
    
    void Start()
    {
        nextSpawnPoint = transform;
        direction = transform.forward;

        tunnelSections[currentSection].sectionLength = Random.Range(tunnelSections[currentSection].sectionLengthMin, tunnelSections[currentSection].sectionLengthMax);

        for (int i = 0; i < startNumberOfTunnels; i++)
        {
            SpawnEmptyTunnel(); //Spawns empty tunnels to start with
        }
        for (int i = 0; i < maxNumberOfTunnels - startNumberOfTunnels; i++)
        {
            SpawnNextTunnel(); //Spawns normal tunnels.
        }
        currentSection = 0;
        ChooseTunnelSection();
    }

    //Spawns empty tunnels to start with
    public void SpawnEmptyTunnel()
    {
        GameObject temp = Instantiate(emptyTunnel, nextSpawnPoint.position, Quaternion.LookRotation(direction));
        nextSpawnPoint = temp.GetComponent<TunnelRespawner>().spawnPoint;
        direction = temp.GetComponent<TunnelRespawner>().spawnPoint.forward;
        SetAllTunnelsMoveSpeed();
    }

    //Spawns Random Tunnel
    public void SpawnNextTunnel()
    {
        /*
        //randomizer = Random.Range(0, tunnelTypes.Count);
        randomizer = Choose(prob);
        GameObject temp = Instantiate(tunnelTypes[randomizer].prefab, nextSpawnPoint.position, Quaternion.LookRotation(direction));
        nextSpawnPoint = temp.GetComponent<TunnelRespawner>().spawnPoint;
        direction = temp.GetComponent<TunnelRespawner>().spawnPoint.forward;
        SetAllTunnelsMoveSpeed();
        */
        //choose tunnel section
        if (tunnelSections[currentSection].tunnelCount >= tunnelSections[currentSection].sectionLength)
        {
            currentSection = nextSection;
            ChooseTunnelSection();
        }
        
        //choose tunnel type to spawn
        if (tunnelSections[currentSection].tunnelCount == 0)
        {
            SpawnStartTunnel();
        }
        else if (tunnelSections[currentSection].tunnelCount < tunnelSections[currentSection].sectionLength - 1)
        {
            SpawnRandomTunnel();
        }
        else
        {
            SpawnTunnelDoor();
        }

        //Increase movement speed over time
        if (moveSpeed < maxSpeed)
        {
            moveSpeed += speedIncrease;
            player.turnSpeed += speedIncrease;
        }
        moveSpeed = Mathf.Clamp(moveSpeed, 0, maxSpeed);
    }

    void ChooseTunnelSection()
    {
        float[] tunnelSectionProbability = new float[tunnelSections.Length];
        for (int i = 0; i < tunnelSections.Length; i++)
        {
            tunnelSectionProbability[i] = tunnelSections[i].rarity;
            if (i == currentSection)
            {
                tunnelSectionProbability[i] = 0;
            }
        }
        //currentSection = WeightedRandom(tunnelSectionProbability);
        nextSection = WeightedRandom(tunnelSectionProbability);

        //Choose random length of section
        tunnelSections[nextSection].sectionLength = Random.Range(tunnelSections[nextSection].sectionLengthMin, tunnelSections[nextSection].sectionLengthMax);
        tunnelSections[nextSection].tunnelCount = 0;
    }

    void SpawnStartTunnel()
    {
        SpawnTunnel(tunnelSections[currentSection].startTunnel);
        tunnelSections[currentSection].tunnelCount++;
    }

    void SpawnRandomTunnel()
    {
        float[] tunnelProbability = new float[tunnelSections[currentSection].tunnelTypes.Length];
        for (int i = 0; i < tunnelSections[currentSection].tunnelTypes.Length; i++)
        {
            tunnelProbability[i] = tunnelSections[currentSection].tunnelTypes[i].rarity;
        }
        int randomTunnel = WeightedRandom(tunnelProbability);
        SpawnTunnel(tunnelSections[currentSection].tunnelTypes[randomTunnel].prefab);
    }

    void SpawnTunnelDoor()
    {
        SpawnTunnel(tunnelSections[currentSection].endTunnel);
        tunnelSections[currentSection].tunnelCount++;
    }

    void SpawnTunnel(GameObject prefab)
    {
        GameObject tunnel = Instantiate(prefab, nextSpawnPoint.position, Quaternion.LookRotation(direction));
        nextSpawnPoint = tunnel.GetComponent<TunnelRespawner>().spawnPoint;
        direction = tunnel.GetComponent<TunnelRespawner>().spawnPoint.forward;

        TunnelDoor door = tunnel.GetComponent<TunnelDoor>();
        if (door != null)
        {
            door.graffitiRenderer.material = tunnelSections[nextSection].graffitiDecal;
        }

        tunnelSections[currentSection].tunnelCount++;

        SetAllTunnelsMoveSpeed();
    }

    //Randomizer
    int WeightedRandom(float[] probs)
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

    public void ChangeMoveSpeed(float deltaSpeed)
    {
        moveSpeed += deltaSpeed;
        SetAllTunnelsMoveSpeed();
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        SetAllTunnelsMoveSpeed();
    }

    //Set the movement speed for every tunnel that exists
    void SetAllTunnelsMoveSpeed()
    {
        TunnelRespawner[] tunnels = FindObjectsOfType<TunnelRespawner>();
        for (int i = 0; i < tunnels.Length; i++)
        {
            tunnels[i].SetMoveSpeed(moveSpeed);
        }
    }
}

//Rarity of the tunnels
[System.Serializable]
public class PrefabArray
{
    public GameObject prefab;
    public float rarity = 1f;
    public float zOffset = 0.25f;
    public Vector2 yawRotation = new Vector2(-180f, 180f);
    public bool ignoreCollisionOnSpawn = false;
    public float collisionCheckRadius = 0.4f;
}

[System.Serializable]
public class TunnelSection
{
    public string name;
    public float rarity = 1f;
    [Range(1, 100)] public int sectionLengthMin = 1;
    [Range(1, 100)] public int sectionLengthMax = 10;
    [HideInInspector] public int sectionLength = 0;
    [HideInInspector] public int tunnelCount = 0;
    public GameObject startTunnel;
    public GameObject endTunnel;
    public Material graffitiDecal;
    public PrefabArray[] tunnelTypes;
}
