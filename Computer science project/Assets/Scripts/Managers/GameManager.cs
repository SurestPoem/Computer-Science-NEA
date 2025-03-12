using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject shopScreen;
    public GameObject DeathScreen;
    public bool ShopOnDev;
    public bool DeathScreenOnDev;

    void Start()
    {
        // Disable screens at the start
        DisableShop();
        DisableDeathScreen();
    }

    void Update()
    {
        // Enable or disable shop based on ShopOnDevThing value
        if (ShopOnDev == true)
        {
            EnableShop();
        }
        else
        {
            DisableShop();
        }

        if (DeathScreenOnDev == true)
        {
            EnableDeathScreen();
        }
        else
        {
            DisableDeathScreen();
        }
    }

    // Method to enable the shop
    public void EnableShop()
    {
        shopScreen.SetActive(true);
        Time.timeScale = 0f; // Freeze time
    }

    // Method to disable the shop
    public void DisableShop()
    {
        shopScreen.SetActive(false);
        Time.timeScale = 1f; // Resume time
    }

    public void EnableDeathScreen()
    {
        DeathScreen.SetActive(true);
        Time.timeScale = 0f; // Freeze time
    }

    public void DisableDeathScreen()
    {
        DeathScreen.SetActive(false);
        Time.timeScale = 1f; // Resume time
    }
}
