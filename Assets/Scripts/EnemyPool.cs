using System.Collections.Generic;
using UnityEngine;
using static EnemyPool;

public class EnemyPool : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPrefab
    {
        public EnemyType type;
        public GameObject prefab;
    }

    public List<EnemyPrefab> enemyPrefabs;
    public int initialPoolSize = 5;

    private Dictionary<EnemyType, Queue<GameObject>> enemyPools = new Dictionary<EnemyType, Queue<GameObject>>();

    void Start()
    {
        foreach (var enemy in enemyPrefabs)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = Instantiate(enemy.prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            enemyPools.Add(enemy.type, pool);
        }
    }

    public GameObject GetEnemy(EnemyType type, Vector3 spawnPosition)
    {
        GameObject enemy;
        if (enemyPools[type].Count > 0)
        {
            enemy = enemyPools[type].Dequeue();
        }
        else
        {
            enemy = Instantiate(enemyPrefabs.Find(e => e.type == type).prefab);
        }

        enemy.transform.position = spawnPosition;
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnEnemy(EnemyType type, GameObject enemy)
    {
        enemy.SetActive(false);
        if (enemyPools.ContainsKey(type))
        {
            enemyPools[type].Enqueue(enemy);
        }
    }
}

public enum EnemyType
{
    Bee,
    Slime,
    Wolf,
    Skeleton,
    Phantom,
    Dragon
}