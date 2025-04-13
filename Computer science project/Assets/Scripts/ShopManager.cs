using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public LevelManager levelManager;
    public List<ShopItem> ShopStock = new List<ShopItem>();
    public List<ShopUpgradeItem> UpgradeStock = new List<ShopUpgradeItem>();

    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }
    public void RemoveGun(int gunIndex)
    {
        if (gunIndex >= 0 && gunIndex < ShopStock.Count)
        {
            Debug.Log(ShopStock[gunIndex].gunPrefab.GetComponent<Gun>().gunName + " removed from shop stock");
            ShopStock.RemoveAt(gunIndex);
            
        }
        else
        {
            Debug.LogError("Gun index out of range");
        }
    }

    public void CloseShopButton()
    {
        if (GameManager.Instance.selectedGameType != GameManager.GameType.Tutorial)
        {
            GameManager.Instance.DisableShop();
        }
        else if (GameManager.Instance.selectedGameType == GameManager.GameType.Normal)
        {
            levelManager.EndStage();
        }
    }
}


