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
    public bool shopEnabled = false;
    public bool deathScreenEnabled = false;
    public bool pauseScreenEnabled = false;


    private void Awake()
    {
        // Ensure only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Optional if you want to keep it across scenes
        }
        else
        {
            Destroy(gameObject);  // If a duplicate exists, destroy it
        }

        DisableShop();
        DisableDeathScreen();
        DisablePauseScreen();
    }



    void Start()
    {

        // Disable screens at the start
          
    }

    void Update()
    {
        //code here idk
    }

    // Method to enable the shop
    public void EnableShop()
    {
        if (shopScreen != null)
        {
            shopScreen.SetActive(true);
            shopEnabled = true;
            Time.timeScale = 0f; // Freeze time
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

    public void QuitGame()
    {
        // Optionally destroy any persistent objects here
        Destroy(GameManager.Instance.gameObject);

        // Quit the application
        Application.Quit();
    }
}


