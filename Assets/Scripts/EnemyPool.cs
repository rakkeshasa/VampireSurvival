using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int initialPoolSize = 50;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    void Start()
    {
        for(int i = 0; i < initialPoolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy(Vector3 spawnPosition)
    {
        GameObject enemy;
        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Dequeue();
        }
        else
        {
            enemy = Instantiate(enemyPrefab); // Ǯ�� ���� �� ������ ���� ����
        }

        enemy.transform.position = spawnPosition;
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
