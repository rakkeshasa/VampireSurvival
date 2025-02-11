using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public float timeToSpawn;
    private float spawnCounter;

    [SerializeField]
    private Transform minSpawn, maxSpawn;

    [SerializeField]
    private EnemyPool enemyPool;

    private Transform target;
    private float despawnDistance;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    public int checkPerFrame; // 이번 프레임에서 체크할 몬스터 수
    private int enemyIndex; // 리스트 인덱스용

    void Start()
    {
        minSpawn = transform.Find("MinSpawnPoint");
        maxSpawn = transform.Find("MaxSpawnPoint");
        spawnCounter = timeToSpawn;

        target = PlayerHealthController.instance.transform;
        despawnDistance = Vector3.Distance(transform.position, maxSpawn.position) + 4f;

        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnCounter);
    }

    void SpawnEnemy()
    {
        GameObject enemy = enemyPool.GetEnemy(SelectSpawnPoint());
    }

    private void Update()
    {
        transform.position = target.position;
    }

    void TempUpdate()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = timeToSpawn;
            GameObject Enemy = Instantiate(enemyToSpawn, SelectSpawnPoint(), transform.rotation);
            spawnedEnemies.Add(Enemy);
        }
        transform.position = target.position;

        // enemyIndex는 0부터 시작 checkPerFrame은 이 프레임에서 체크할 몬스터 수
        // int checkTarget = enemyIndex + checkPerFrame;
        int checkTarget = checkPerFrame;
        while (enemyIndex < checkTarget)
        {
            if (enemyIndex >= spawnedEnemies.Count)
            {
                enemyIndex = 0;
                checkTarget = 0;
            }

            if (spawnedEnemies[enemyIndex] == null)
                break;


            if (Vector3.Distance(transform.position, spawnedEnemies[enemyIndex].transform.position) > despawnDistance)
            {
                // 이번 몬스터는 거리 밖에 있다 파괴
                Destroy(spawnedEnemies[enemyIndex]);
                spawnedEnemies.RemoveAt(enemyIndex);

                // 체크 했으니 체크할 몬스터 - 1
                checkTarget--;
            }
            else
            {
                // 이번 몬스터는 거리 내에 있다 다음 몬스터
                enemyIndex++;
            }
        }
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        bool spawnVerticalEdge = Random.Range(0f, 1f) > .5f;

        if(spawnVerticalEdge)
        {
            spawnPoint.y = Random.Range(minSpawn.position.y, maxSpawn.position.y);

            if (Random.Range(0f, 1f) > .5f)
                spawnPoint.x = maxSpawn.position.x;
            else
                spawnPoint.x = minSpawn.position.x;
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);

            if (Random.Range(0f, 1f) > .5f)
                spawnPoint.y = maxSpawn.position.y;
            else
                spawnPoint.y = minSpawn.position.y;
        }

        return spawnPoint;
    }
}
