using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    public float spawnRate = 2f;
    private float nextSpawnTime = 0f;
    public int enemiesPerSpawn = 3;
    public Tilemap tileMap;
    public RuleTile Floor;

    // Reference to the WalkerGenerator (adjust the reference to match your game)
    public WalkerGenerator walkerGenerator;

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
            // Try to spawn the enemy
            Vector3 spawnPosition = GetValidSpawnPosition();

            if (spawnPosition != Vector3.zero) // If a valid spawn position was found
            {
                Debug.Log("Spawning enemy at: " + spawnPosition);
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                //No code nessisary here
            }
        }
    }

    // Recursively get a valid spawn position
    Vector3 GetValidSpawnPosition(int maxAttempts = 10)
    {
        if (maxAttempts <= 0)
        {
            return Vector3.zero; // No valid position found, return a "failure" value
        }

        Vector3Int spawnTilePosition = new Vector3Int(
            Random.Range(tileMap.cellBounds.min.x, tileMap.cellBounds.max.x),
            Random.Range(tileMap.cellBounds.min.y, tileMap.cellBounds.max.y),
            0);

        Vector3 spawnPosition = tileMap.CellToWorld(spawnTilePosition) + tileMap.tileAnchor;

        TileBase tileAtPosition = tileMap.GetTile(spawnTilePosition);

        if (tileAtPosition == Floor)
        {
            return spawnPosition; // Return valid position
        }
        else
        {
            // Recurse with one fewer attempt
            return GetValidSpawnPosition(maxAttempts - 1);
        }
    }
}