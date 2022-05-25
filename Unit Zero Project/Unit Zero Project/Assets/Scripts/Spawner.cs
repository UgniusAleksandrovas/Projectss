using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public float waitTillNextWave;
    public List<GameObject> spawnPrefabs;
    public List<Transform> spawnPositions;

    private void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            spawnPositions.Add(transform.GetChild(i));
        }
    }

    public void Spawn(int numWaves) {
        StartCoroutine(SpawnWaves(numWaves));
    }

    private IEnumerator SpawnWaves(int waves) {
        for (int i = 0; i < waves; i++) {
            foreach (Transform spawnPos in spawnPositions) {
                RaycastHit hit;
                if (Physics.Raycast(spawnPos.position, Vector3.down, out hit)) {
                    GameObject prefab = Instantiate(spawnPrefabs[Random.Range(0, spawnPrefabs.Count)], hit.point, spawnPos.transform.rotation);
                    if (prefab.GetComponent<SoldierAI>()) {
                        prefab.GetComponent<SoldierAI>().combatTime = 100f;
                        prefab.GetComponent<SoldierAI>().sneak = true;
                    }
                }
                else {
                    continue;
                }
            }
            yield return new WaitForSeconds(waitTillNextWave);
        }
    }
}
