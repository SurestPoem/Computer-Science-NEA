using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject shopScreen;
    public GameObject deathScreen;
    public GameObject pauseScreen;
    [HideInInspector] public bool shopEnabled = false;
    [HideInInspector] public bool deathScreenEnabled = false;
    [HideInInspector] public bool pauseScreenEnabled = false;
    public enum Difficulty { Easy, Normal, Hard }
    public Difficulty currentDifficulty;
    public float difficultyMultiplier;

    public enum GameType { Normal, Endless, Tutorial }
    public GameType selectedGameType;  // Hold the player's selected game mode

    private void Awake()
    {
        // Ensure only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // If a duplicate exists, destroy it
        }

        // Subscribe to scene loaded event to reassign UI elements
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure the UI elements are reinitialized after a scene is loaded
        
        StartCoroutine(InitializeUI());

        if (shopScreen == null || deathScreen == null || pauseScreen == null)
        {
            Debug.LogError("Some UI screens are missing!");
        }

        // Reset UI states (ensure they're all disabled at scene load)
        DisableShop();
        DisableDeathScreen();
        DisablePauseScreen();
    }

    public void StartGame(int gameMode)
    {
        selectedGameType = (GameType)gameMode;

        // Load the correct scene based on game mode
        string sceneToLoad = (selectedGameType == GameType.Normal) ? "Game" : "Endless";
        SceneManager.LoadScene(sceneToLoad);
        SetDifficultyMultiplier();
    }

    public void SetDifficulty(int sliderValue)
    {
        if (sliderValue == 0)
        {
            currentDifficulty = Difficulty.Easy;
        }
        else if (sliderValue == 1)
        {
            currentDifficulty = Difficulty.Normal;
        }
        else
        {
            currentDifficulty = Difficulty.Hard;
        }
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

    // Shop functionality
    public void EnableShop()
    {
        if (shopScreen != null)
        {
            shopScreen.SetActive(true);
            shopEnabled = true;
            Time.timeScale = 0f; // Freeze time

            // Loop through each ShopNode and call PickSellableType
            ShopNode[] shopNodes = shopScreen.GetComponentsInChildren<ShopNode>();
            foreach (ShopNode shopNode in shopNodes)
            {
                shopNode.PickSellableType(); // Initialize the sellable items
            }
        }
        else
        {
            Debug.LogWarning("ShopScreen is null or has been destroyed.");
        }
    }

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

    // Death screen functionality
    public void EnableDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
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
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
            deathScreenEnabled = false;
            Time.timeScale = 1f; // Resume time
        }
        else
        {
            Debug.LogWarning("Death screen has been destroyed!");
        }
    }

    // Pause screen functionality
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

    // Restart the game
    public void RestartGame()
    {
        ResetValues();
        StartGame((int)selectedGameType);
    }

    // Load the main menu
    public void LoadMainMenu()
    {
        ResetValues();
        // Load the MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator InitializeUI()
    {
        yield return new WaitForSeconds(0.1f);
        if (shopScreen == null)
        {
            shopScreen = GameObject.FindGameObjectWithTag("ShopScreen");
            if (shopScreen == null)
            {
                Debug.LogWarning("ShopScreen is missing.");
            }
        }

        if (deathScreen == null)
        {
            deathScreen = GameObject.FindGameObjectWithTag("DeathScreen");
            if (deathScreen == null)
            {
                Debug.LogWarning("DeathScreen is missing.");
            }
        }

        if (pauseScreen == null)
        {
            pauseScreen = GameObject.FindGameObjectWithTag("PauseScreen");
            if (pauseScreen == null)
            {
                Debug.LogWarning("PauseScreen is missing.");
            }
        }
    }

    private void ResetValues()
    {
        Time.timeScale = 1f;
        if (deathScreen != null && deathScreenEnabled == true) DisableDeathScreen();
        if (shopScreen != null && shopEnabled == true) DisableShop();
        if (pauseScreen != null && pauseScreenEnabled == true) DisablePauseScreen();
        shopScreen = null;
        deathScreen = null;
        pauseScreen = null;
        difficultyMultiplier = 1f;
    }

    // Quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        // Quit the application
        Application.Quit();
    }
}