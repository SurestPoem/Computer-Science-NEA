using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    private float spawnRate = 2f;  // Spawn every 2 seconds
    private float nextSpawnTime = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    public void SpawnEnemy()
    {
        // Create an offset to randomize the spawn location
        Vector3 offset = new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), 0f);

        // Apply the offset to the spawn position
        Vector3 spawnPosition = transform.position + offset;

        // Instantiate the enemy at the new position
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }


}