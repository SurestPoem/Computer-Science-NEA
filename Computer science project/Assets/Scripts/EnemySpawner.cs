using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemySpawner : MonoBehaviour
{
    public List<EnemySpawnItem> avalibleEnemies = new List<EnemySpawnItem>();
    public List<EnemySpawnItem> enemyTypes = new List<EnemySpawnItem>();
    [SerializeField] public GameObject enemyPrefab;
    public float spawnRate = 2f;
    private float nextSpawnTime = 0f;
    public int enemiesPerSpawn = 3;
    public Tilemap tileMap;
    public RuleTile Floor;

    // Reference to the WalkerGenerator (adjust the reference to match your game)
    public WalkerGenerator walkerGenerator;

    void Awake()
    {
        walkerGenerator = FindObjectOfType<WalkerGenerator>();
        tileMap = FindObjectOfType<Tilemap>();
        spawnRate = spawnRate / GameManager.Instance.difficultyMultiplier;
        enemiesPerSpawn = Mathf.RoundToInt(enemiesPerSpawn * GameManager.Instance.difficultyMultiplier);
    }

    void Start()
    {
        nextSpawnTime = Time.time + spawnRate/2; // Start the first spawn after half the spawn rate
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            StartCoroutine(SpawnEnemies());
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    public IEnumerator SpawnEnemies()
    {
        PickEnemyType();
        int maxAttempts = walkerGenerator.MapWidth * walkerGenerator.MapHeight; // Ensure this is set correctly

        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(maxAttempts);

            if (spawnPosition != Vector3.zero) // If a valid spawn position was found
            {
                Debug.Log("Spawning enemy at: " + spawnPosition);
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
            //Waits a short time before spawning the next enemy
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Recursively get a valid spawn position
    Vector3 GetValidSpawnPosition(int maxAttempts)
    {
        if (maxAttempts <= 0)
        {
            return Vector3.zero; // No valid position found
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

    public void PickEnemyType()
    {
        float totalChance = 0;
        foreach (EnemySpawnItem enemy in enemyTypes)
        {
            totalChance += enemy.spawnChance;
        }
        float randomValue = Random.Range(0, totalChance);
        foreach (EnemySpawnItem enemy in enemyTypes)
        {
            if (randomValue < enemy.spawnChance)
            {
                if (!enemy.isEnabled)
                {
                    Debug.Log("Enemy is disabled, skipping.");
                    PickEnemyType();
                }
                else
                {
                    enemyPrefab = enemy.enemyPrefab;
                    enemiesPerSpawn = enemy.spawnAmount;
                    return;
                }
                    
            }
            randomValue -= enemy.spawnChance;
        }
        enemiesPerSpawn = Mathf.RoundToInt(enemiesPerSpawn * GameManager.Instance.difficultyMultiplier);
    }

    private void CheckSpawnArea()
    {
        //Checks if the area around the spawn point meets minSpawnArea
      
    }
}