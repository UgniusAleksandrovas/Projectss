using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Multiplayer
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] bool spawnOnStart = false;
        [SerializeField] NetworkObject playerPrefab;
        [SerializeField] Vector3Range spawnPositionRange = new Vector3Range(new Vector3(-5, 0, -5), new Vector3(5, 0, 5));
        [SerializeField] Vector2 spawnRotationRange = new Vector2(-180f, 180f);

        private List<ulong> playerObjectIds;

        public delegate void SpawnedPlayers();
        public static event SpawnedPlayers OnSpawnedPlayers;

        private void Awake()
        {
            playerObjectIds = new List<ulong>();
        }

        void Start()
        {
            if (spawnOnStart)
            {
                StartCoroutine(SpawnAllPlayerObjects());
            }
        }

        public IEnumerator SpawnAllPlayerObjects()
        {
            yield return new WaitForSeconds(0.1f);
            SpawnPlayersServerRpc();
            if (OnSpawnedPlayers != null)
            {
                OnSpawnedPlayers();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnPlayersServerRpc()
        {
            foreach (ulong player in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playerObjectIds.Contains(player))
                {
                    Vector3 spawnPos = new Vector3(
                        Random.Range(spawnPositionRange.min.x, spawnPositionRange.max.x),
                        Random.Range(spawnPositionRange.min.y, spawnPositionRange.max.y),
                        Random.Range(spawnPositionRange.min.z, spawnPositionRange.max.z)
                    );
                    Quaternion spawnRot = Quaternion.Euler(0f, Random.Range(spawnRotationRange.x, spawnRotationRange.y), 0f);

                    NetworkObject playerInstance = Instantiate(playerPrefab, transform.position + spawnPos, spawnRot);

                    playerInstance.SpawnAsPlayerObject(player, true);

                    playerObjectIds.Add(player);
                }
            }
        }
    }
}
