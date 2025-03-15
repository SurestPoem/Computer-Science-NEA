using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    public float spawnRate = 2f;  // Spawn every 2 seconds
    private float nextSpawnTime = 0f;

    public int enemiesPerSpawn = 3; // Number of enemies to spawn each time

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemies();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), 0f);
            Vector3 spawnPosition = transform.position + offset;
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
