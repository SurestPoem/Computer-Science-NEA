using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject shopScreen;
    public bool ShopOnDevThing;  // True to show shop, false to hide

    void Start()
    {
        // Disable shop at the start
        DisableShop();
    }

    void Update()
    {
        // Enable or disable shop based on ShopOnDevThing value
        if (ShopOnDevThing)
        {
            EnableShop();
        }
        else
        {
            DisableShop();
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
}
