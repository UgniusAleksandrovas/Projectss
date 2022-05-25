using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class TunnelManager : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] float maxSpeed = 30f;
        public float moveSpeed = 6f;
        [SerializeField] float speedIncrease = 0.05f;

        [Header("Tunnel Settings")]
        [SerializeField] int maxNumberOfTunnels = 10; //max number of tunnels at a time
        [SerializeField] int startNumberOfTunnels = 5; //max number of tunnels at the beginning of the game
        [SerializeField] GameObject emptyTunnel;
        [SerializeField] PrefabArray[] tunnelTypes; //the rarity of each tunnel

        float[] prob;
        int randomizer;

        Transform nextSpawnPoint;
        Vector3 direction;

        List<PlayerControllerMultiplayer> players;

        void Awake()
        {
            prob = new float[tunnelTypes.Length];
            for (int i = 0; i < tunnelTypes.Length; i++)
            {
                prob[i] = tunnelTypes[i].rarity;
            }
            players = new List<PlayerControllerMultiplayer>();
            PlayerSpawner.OnSpawnedPlayers += SetPlayers;
        }

        void SetPlayers()
        {
            PlayerControllerMultiplayer[] playersArray = FindObjectsOfType<PlayerControllerMultiplayer>();
            for (int i = 0; i < playersArray.Length; i++)
            {
                players.Add(playersArray[i]);
            }
        }

        void Start()
        {
            nextSpawnPoint = transform;
            direction = transform.forward;
            for (int i = 0; i < startNumberOfTunnels; i++)
            {
                SpawnEmptyTunnel(); //Spawns empty tunnels to start with
            }
            for (int i = 0; i < maxNumberOfTunnels - startNumberOfTunnels; i++)
            {
                SpawnNextTunnel(); //Spawns normal tunnels.
            }
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
            //randomizer = Random.Range(0, tunnelTypes.Count);
            randomizer = Choose(prob);
            GameObject temp = Instantiate(tunnelTypes[randomizer].prefab, nextSpawnPoint.position, Quaternion.LookRotation(direction));
            nextSpawnPoint = temp.GetComponent<TunnelRespawner>().spawnPoint;
            direction = temp.GetComponent<TunnelRespawner>().spawnPoint.forward;
            SetAllTunnelsMoveSpeed();

            //Increase movement speed over time
            if (moveSpeed < maxSpeed)
            {
                moveSpeed += speedIncrease;
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].turnSpeed += speedIncrease;
                }
            }
            moveSpeed = Mathf.Clamp(moveSpeed, 0, maxSpeed);
        }

        //Randomizer
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
}
