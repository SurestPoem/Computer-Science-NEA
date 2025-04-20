using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnItem
{
    public GameObject enemyPrefab;
    public int spawnAmount;
    public float spawnChance;
    public int minSpawnArea;
}
