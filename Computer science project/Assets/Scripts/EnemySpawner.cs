using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    public float spawnRate = 2f;  // Spawn every 2 seconds
    private float nextSpawnTime = 0f;
    public int enemiesPerSpawn = 3; // Number of enemies to spawn each time

    public Tilemap tileMap; // Reference to the Tilemap
    public RuleTile Floor; // Reference to the floor tile

    void Start()
    {
        spawnRate = spawnRate / GameManager.Instance.difficultyMultiplier;
        enemiesPerSpawn = Mathf.RoundToInt(enemiesPerSpawn * GameManager.Instance.difficultyMultiplier);
    }

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
            // Generate random offset for spawn position
            Vector3 offset = new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), 0f);
            Vector3 spawnPosition = transform.position + offset;

            // Convert the spawn position to a Vector3Int for tilemap usage
            Vector3Int spawnTilePosition = tileMap.WorldToCell(spawnPosition);

            // Check if the tile at the spawn position is a floor
            TileBase tileAtPosition = tileMap.GetTile(spawnTilePosition);

            if (tileAtPosition == Floor) // Only spawn if it's a floor tile
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}