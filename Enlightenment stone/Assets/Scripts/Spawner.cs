using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Enemy;
    public int xPos;
    public int zPos;
    public int enemyCount;
    public GameObject Boss;
    public bool checkToSpawnBoss;
    public GameObject anotherLevelBoss;
    public GameObject thirdLevelBoss;

    public bool canSpawn;

    private void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    public void Update()
    {
        if (checkToSpawnBoss == true)
        {
            if(enemyCount == 0)
            {
                Boss.SetActive(true);
                StartCoroutine(moonstertwo());
                StartCoroutine(Monsterthree());
            }
        }
    }
    IEnumerator moonstertwo()
    {
        yield return new WaitForSeconds(4f);
        anotherLevelBoss.SetActive(true);
    }

    IEnumerator Monsterthree()
    {
        yield return new WaitForSeconds(4f);
        thirdLevelBoss.SetActive(true);
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 10)
        {
                xPos = Random.Range(-30, -1);
                zPos = Random.Range(89, 130);
                GameObject obj = Instantiate(Enemy, new Vector3(xPos, 0, zPos), Quaternion.identity);
                obj.GetComponent<EnemyAi>().spawnScript = this;
                yield return new WaitForSeconds(0.5f);
                enemyCount += 1;
                checkToSpawnBoss = true;
            }
    }
}

