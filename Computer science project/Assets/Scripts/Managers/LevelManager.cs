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
    public GameObject tutorialCheckpoint;
    private List<string> tutorialMessages = new List<string>()
    {
        "Use WASD to move around.",                                                             // Stage 1
        "Using the mouse to move the crosshair \n use left click to shoot and kill the charger",  // Stage 2
        "You took damage, \n pick up the health pack to heal",                                     // Stage 3
        "Press Q to open the shop and buy \n something with the currency just given to you",       // Stage 4
        "Now defeat the four slimes",                                                           // Stage 5
        "You are now done with the tutorial."                                                   // Stage 6
    };
    public string currentTutorialMessage = "";


    void Awake()
    {
        player = FindObjectOfType<Player>();
        if (GameManager.Instance.selectedGameType == GameManager.GameType.Normal || GameManager.Instance.selectedGameType == GameManager.GameType.Endless)
        {
            walkerGenerator = FindObjectOfType<WalkerGenerator>();
            enemySpawner = GameObject.Find("EnemySpawner");
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartStages();
    }

    public void NextStage()
    {
        StartCoroutine(StartNewStage());
    }

    public void EndStage()
    {
        
        if (GameManager.Instance.selectedGameType == GameManager.GameType.Normal)
        {
            if (currentStage < stageToWin)
            {
                currentStage++;
                enemySpawner.SetActive(false);
                GameManager.Instance.EnableShop();
                player.health = player.maxHealth;
                ClearObjects();
            }
            else if (currentStage >= stageToWin)
            {
                WinGame();
            }

        }
        else if (GameManager.Instance.selectedGameType == GameManager.GameType.Endless)
        {
            currentStage++;
            player.health = player.maxHealth;
            NextStage();
        }
        else if (GameManager.Instance.selectedGameType == GameManager.GameType.Tutorial)
        {            
            currentStage++;
            StartCoroutine(StartNewStage());            
        }
        else
        {
            Debug.LogError("Invalid game type selected.");
        }
    }

    public void StartStages()
    {
        currentStage = 1;
        objectiveComplete = false;
        
        if (GameManager.Instance.selectedGameType == GameManager.GameType.Tutorial)
        {
            currentStage = 1;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().IsFreeze = true;
            }
            StartNextTutorialStage();
        }
    }

    public IEnumerator StartNewStage()
    {
        objectiveComplete = false;
        GameManager.Instance.AddDifficultyMultiplier();
        if (GameManager.Instance.selectedGameType == GameManager.GameType.Normal)
        {            
            GameManager.Instance.DisableShop();
            walkerGenerator.ResetGrid();
            killGoal = 30 + (currentStage * 2);
            enemySpawner.SetActive(true);
            yield return null;
        }
        else if (GameManager.Instance.selectedGameType == GameManager.GameType.Endless)
        {
            killGoal = 30 + (currentStage * 2);
            yield return null;
        }
        else if (GameManager.Instance.selectedGameType == GameManager.GameType.Tutorial)
        {
            StartNextTutorialStage();
            yield return null;
        }
        else
        {
            Debug.LogError("Invalid game type selected.");
        }
    }

    public void WinGame()
    {
        objectiveComplete = true;
        GameManager.Instance.DisableShop();
        GameManager.Instance.DisablePauseScreen();
        GameManager.Instance.EnableWinScreen();
    }

    public void DecreaseKillGoal()
    {
        killGoal--;
        if (GameManager.Instance.selectedGameType != GameManager.GameType.Tutorial)
        {
            if (killGoal <= 0 && !objectiveComplete)
            {
                objectiveComplete = true;
                EndStage();
            }
        }
    }

    public void StartNextTutorialStage()
    {
        ShowTutorialMessage();
        if (currentStage == 3)
        {
            player.takeDamage(20f);
        }
        if (currentStage == 4)
        {
            player.EarnCurrency(300);
            player.EarnXP(5000);
            //Gives player enough level and currency to buy something from the shop
        }
        if (currentStage == 5)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().IsFreeze = false;
            }
        }
        if (currentStage == 6)
        {
            WinGame();
        }     
    }
    void ShowTutorialMessage()
    {
        if (currentStage - 1 < tutorialMessages.Count)
        {
            currentTutorialMessage = tutorialMessages[currentStage - 1];
            Debug.Log("Tutorial Message: " + currentTutorialMessage);
           
        }
    }

    private void ClearObjects()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int enemiesDestroyed = 0;
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
            enemiesDestroyed++;
        }
        GameObject[] dropables = GameObject.FindGameObjectsWithTag("Dropable");
        int dropablesDestroyed = 0;
        foreach (GameObject dropable in dropables)
        {
            Destroy(dropable);
            dropablesDestroyed++;
        }
        Debug.Log("Enemies destroyed: " + enemiesDestroyed + " and Dropables destroyed: " + dropablesDestroyed);
    }
}
