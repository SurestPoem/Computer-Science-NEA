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
            // Get a random tile position within the Tilemap bounds
            Vector3Int spawnTilePosition = new Vector3Int(
                Random.Range(tileMap.cellBounds.min.x, tileMap.cellBounds.max.x),
                Random.Range(tileMap.cellBounds.min.y, tileMap.cellBounds.max.y),
                0);

            // Convert the tile position to a world position
            Vector3 spawnPosition = tileMap.CellToWorld(spawnTilePosition) + tileMap.tileAnchor;

            // Check if the tile at the spawn position is a floor tile
            TileBase tileAtPosition = tileMap.GetTile(spawnTilePosition);

            if (tileAtPosition == Floor) // Only spawn if it's a floor tile
            {
                Debug.Log("Spawning enemy at: " + spawnPosition);
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}