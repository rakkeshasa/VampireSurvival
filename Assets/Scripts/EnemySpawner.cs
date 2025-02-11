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
    public int checkPerFrame; // �̹� �����ӿ��� üũ�� ���� ��
    private int enemyIndex; // ����Ʈ �ε�����

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

        // enemyIndex�� 0���� ���� checkPerFrame�� �� �����ӿ��� üũ�� ���� ��
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
                // �̹� ���ʹ� �Ÿ� �ۿ� �ִ� �ı�
                Destroy(spawnedEnemies[enemyIndex]);
                spawnedEnemies.RemoveAt(enemyIndex);

                // üũ ������ üũ�� ���� - 1
                checkTarget--;
            }
            else
            {
                // �̹� ���ʹ� �Ÿ� ���� �ִ� ���� ����
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
