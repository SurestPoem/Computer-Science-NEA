using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject shopScreen;
    public GameObject DeathScreen;
    public GameObject pauseScreen;
    public bool ShopOnDev;
    public bool DeathScreenOnDev;
    [HideInInspector] public bool shopEnabled = false;
    [HideInInspector] public bool deathScreenEnabled = false;
    [HideInInspector] public bool pauseScreenEnabled = false;
    public enum Difficulty {Easy, Normal, Hard}
    public Difficulty currentDifficulty;
    public float difficultyMultiplier;

    public enum GameType { Normal, Endless }
    public GameType selectedGameType;  // Hold the player's selected game mode



    private void Awake()
    {
        // Ensure only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(true);
        }
        else
        {
            Destroy(gameObject);  // If a duplicate exists, destroy it
        }
    }



    public void StartGame(int gameMode)
    {
        selectedGameType = (GameType)gameMode;

        // Load the correct scene based on game mode
        string sceneToLoad = (selectedGameType == GameType.Normal) ? "Game" : "Endless";
        SceneManager.LoadScene(sceneToLoad);


        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        // Start a coroutine to wait for the scene to load before assigning UI elements
        StartCoroutine(AssignSceneUI());
    }


    public void SetDifficultyMultiplier()
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy:
                    difficultyMultiplier = 0.65f;
                    break;
                case Difficulty.Normal:
                    difficultyMultiplier = 1f;
                    break;
                case Difficulty.Hard:
                    difficultyMultiplier = 1.15f;
                    break;
            }
        }

    public void EnableShop()
    {
        if (shopScreen != null)
        {
            shopScreen.SetActive(true); // Activate the shop screen
            shopEnabled = true;

            // Freeze time
            Time.timeScale = 0f;

            // Find all ShopNode components that are children of the shopScreen
            ShopNode[] shopNodes = shopScreen.GetComponentsInChildren<ShopNode>();

            // Loop through each ShopNode and call PickSellableType()
            foreach (ShopNode shopNode in shopNodes)
            {
                shopNode.PickSellableType(); // Call the method for each ShopNode
            }
        }
        else
        {
            Debug.LogWarning("ShopScreen is null or has been destroyed.");
        }
    }

    // Method to disable the shop
    public void DisableShop()
    {
        if (shopScreen != null)
        {
            shopScreen.SetActive(false);
            shopEnabled = false;
            Time.timeScale = 1f; // Resume time
        }
        else
        {
            Debug.LogWarning("Shop screen has been destroyed!");
        }
    }

    public void EnableDeathScreen()
    {
        if (DeathScreen != null)
        {
            DeathScreen.SetActive(true);
            deathScreenEnabled = true;
            Time.timeScale = 0f; // Freeze time
        }
        else
        {
            Debug.LogWarning("DeathScreen is null or has been destroyed.");
        }
    }

    public void DisableDeathScreen()
    {
        if (DeathScreen != null)
        {
            DeathScreen.SetActive(false);
            deathScreenEnabled = false;
            Time.timeScale = 1f; // Resume time
        }
        else
        {
            Debug.LogWarning("Death screen has been destroyed!");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnablePauseScreen()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
            pauseScreenEnabled = true;
            Time.timeScale = 0f; // Freeze time
        }
        else
        {
            Debug.LogWarning("PauseScreen is null or has been destroyed.");
        }
    }

    public void DisablePauseScreen()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
            pauseScreenEnabled = false;
            Time.timeScale = 1f; // Resume time
        }
        else
        {
            Debug.LogWarning("Pause screen has been destroyed!");
        }
    }

    private IEnumerator AssignSceneUI()
    {
        // Find UI elements in the new scene
        shopScreen = GameObject.FindWithTag("ShopScreen");
        DeathScreen = GameObject.FindWithTag("DeathScreen");
        pauseScreen = GameObject.FindWithTag("PauseScreen");

        if (shopScreen == null) Debug.LogWarning("ShopScreen not found in scene!");
        if (DeathScreen == null) Debug.LogWarning("DeathScreen not found in scene!");
        if (pauseScreen == null) Debug.LogWarning("PauseScreen not found in scene!");

        if(shopScreen) DisableShop();
        if(DeathScreen) DisableDeathScreen();
        if (pauseScreen) DisablePauseScreen();

        yield return null;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        // Optionally destroy any persistent objects here
        Destroy(GameManager.Instance.gameObject);

        // Quit the application
        Application.Quit();
    }
}


