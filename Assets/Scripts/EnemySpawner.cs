using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // public GameObject enemyToSpawn;
    // public float timeToSpawn;
    private float spawnCounter;

    [SerializeField]
    private Transform minSpawn, maxSpawn;
    private Transform target;
    // private List<GameObject> spawnedEnemies = new List<GameObject>();

    public List<WaveInfo> waves;
    private int currentWave;
    private float waveCounter;

    void Start()
    {
        minSpawn = transform.Find("MinSpawnPoint");
        maxSpawn = transform.Find("MaxSpawnPoint");
        target = PlayerHealthController.instance.transform;

        spawnCounter = waves[0].spawnInterval;
        waveCounter = waves[0].waveDuration;
    }


    private void Update()
    {
        if (PlayerHealthController.instance.gameObject.activeSelf)
        {
            if (currentWave < waves.Count)
            {
                waveCounter -= Time.deltaTime;
                if (waveCounter <= 0)
                {
                    GoToNextWave();
                }

                spawnCounter -= Time.deltaTime;
                if (spawnCounter <= 0)
                {
                    spawnCounter = waves[currentWave].spawnInterval;
                    GameObject newEnemy = Instantiate(waves[currentWave].enemy, SelectSpawnPoint(), Quaternion.identity);
                    // spawnedEnemies.Add(newEnemy);
                }
            }
        }

        transform.position = target.position;
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

    public void GoToNextWave()
    {
        waves[currentWave].spawnInterval -= .2f;
        waves[currentWave].spawnInterval = Mathf.Max(waves[currentWave].spawnInterval, .25f);
        currentWave++;
        // 마지막 웨이브일 시
        if(currentWave >= waves.Count)
        {
            currentWave = 0;
        }

        waveCounter = waves[currentWave].waveDuration;
        spawnCounter = waves[currentWave].spawnInterval;
    }
}

[System.Serializable]
public class WaveInfo
{
    public GameObject enemy;
    public float waveDuration = 10f;
    public float spawnInterval = 1f;
}