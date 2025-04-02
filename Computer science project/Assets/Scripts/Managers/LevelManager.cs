using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Player player;
    public WalkerGenerator walkerGenerator;
    public GameObject enemySpawner;
    public int currentStage = 1;
    public bool objectiveComplete = false;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        walkerGenerator = FindObjectOfType<WalkerGenerator>();
        enemySpawner = GameObject.Find("EnemySpawner");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartStages();
    }

    void Update()
    {
        if (objectiveComplete)
        {
            NextStage();
        }
    }

    public void NextStage()
    {
        StartCoroutine(StartNewStage());
    }

    public void EndStage()
    {
        enemySpawner.SetActive(false);
        GameManager.Instance.EnableShop();
        player.health = player.maxHealth;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy); // Destroy each enemy
        }
    }

    public void StartStages()
    {
        currentStage = 1;
        objectiveComplete = false;
    }

    public IEnumerator StartNewStage()
    {
        objectiveComplete = false;
        currentStage++;
        GameManager.Instance.DisableShop();
        GameManager.Instance.AddDifficultyMultiplier();
        walkerGenerator.ResetGrid();
        yield return new WaitForSeconds(0.1f);
        
        enemySpawner.SetActive(true);
    }
}
