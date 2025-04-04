using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public Player player;
    public WalkerGenerator walkerGenerator;
    public GameObject enemySpawner;
    public int currentStage = 1;
    public int stageToWin = 5;
    public bool objectiveComplete = false;
    public int killGoal = 30;


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

    }

    public void NextStage()
    {
        StartCoroutine(StartNewStage());
    }

    public void EndStage()
    {
        if (GameManager.Instance.selectedGameType == GameManager.GameType.Normal)
        {
            enemySpawner.SetActive(false);
            GameManager.Instance.EnableShop();
            player.health = player.maxHealth;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            int enemiesDestroyed = 0;
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
                enemiesDestroyed += 1;
            }
            Debug.Log("Enemies destroyed: " + enemiesDestroyed);
            GameObject[] dropables = GameObject.FindGameObjectsWithTag("Dropables");
            foreach(GameObject dropable in dropables)
            {
                Destroy(dropable);
            }
            
        }

        else if (GameManager.Instance.selectedGameType == GameManager.GameType.Endless)
        {
            player.health = player.maxHealth;
            NextStage();
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
        GameManager.Instance.AddDifficultyMultiplier();
        if (GameManager.Instance.selectedGameType == GameManager.GameType.Normal)
        {            
            GameManager.Instance.DisableShop();
            walkerGenerator.ResetGrid();
            killGoal = 30 + (currentStage * 5);
            enemySpawner.SetActive(true);
            yield return null;
        }
        else if (GameManager.Instance.selectedGameType == GameManager.GameType.Endless)
        {
            killGoal = 30 + (currentStage * 2);
            yield return null;
        }
        
    }

    public void DecreaseKillGoal()
    {
        killGoal--;
        if (killGoal <= 0 && !objectiveComplete)
        {
            objectiveComplete = true;
            EndStage();
        }
    }
}
