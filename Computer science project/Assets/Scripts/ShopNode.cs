using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopNode : MonoBehaviour
{
    public Player player;
    public enum SellableType { Gun, Upgrade, Null }
    public SellableType currentSellableType;
    public int generalCost;
    public int gunPointer = 0;
    public int upgradePointer = 0;
    public ShopManager shopManager;

    void Start()
    {
        player = FindObjectOfType<Player>();
        PickSellableType();
    }


   
    public void BuyGun()
    {
        if (player.currentCurrency >= generalCost && player.level >= shopManager.ShopStock[gunPointer].levelRequired)
        {
            player.AddGun(shopManager.ShopStock[gunPointer].gunPrefab);
            shopManager.RemoveGun(gunPointer);
            gunPointer = 0;
            player.UseCurrency(generalCost);
            currentSellableType = SellableType.Null;
        }
        else
        {
            Debug.Log("Player does not have enough currency or high enough level");
        }
    }

    public void BuyUpgrade()
    {
        if (player.currentCurrency >= generalCost)
        {
            ShopUpgradeItem.UpgradeType upgradeType = shopManager.UpgradeStock[upgradePointer].upgradeType;

            switch (upgradeType)
            {
                case ShopUpgradeItem.UpgradeType.MaxHealth:
                    player.IncreaseStats("maxHealth", shopManager.UpgradeStock[upgradePointer].upgradeAmount);
                    break;
                case ShopUpgradeItem.UpgradeType.MoveSpeed:
                    player.IncreaseStats("moveSpeed", shopManager.UpgradeStock[upgradePointer].upgradeAmount);
                    break;
                case ShopUpgradeItem.UpgradeType.RegenRate:
                    player.IncreaseStats("regenRate", shopManager.UpgradeStock[upgradePointer].upgradeAmount);
                    break;
                default:
                    Debug.LogError("Unknown upgrade type.");
                    return;
            }
            player.UseCurrency(generalCost);
            currentSellableType = SellableType.Null;
        }
        else
        {
            Debug.Log("Player is broke, not enough currency");
        }
    }


    public void RandomiseUpgradeValues()
    {
        ShopUpgradeItem.UpgradeType upgradeType = shopManager.UpgradeStock[upgradePointer].upgradeType;

        switch (upgradeType)
        {
            case ShopUpgradeItem.UpgradeType.MaxHealth:
                // Randomize the amount of max health
                shopManager.UpgradeStock[upgradePointer].upgradeAmount = Random.Range(1, 7);
                break;

            case ShopUpgradeItem.UpgradeType.MoveSpeed:
                // Randomize move speed upgrade
                shopManager.UpgradeStock[upgradePointer].upgradeAmount = Random.Range(1, 4);
                break;

            case ShopUpgradeItem.UpgradeType.RegenRate:
                // Randomize regen rate upgrade
                shopManager.UpgradeStock[upgradePointer].upgradeAmount = Random.Range(1, 4);
                break;

            default:
                Debug.LogError("Unknown upgrade type in randomization.");
                break;
        }
    }



    public void PickSellableType()
    {
        if (shopManager.ShopStock.Count == 0)
        {
            currentSellableType = SellableType.Upgrade;
        }
        else
        {
            // 2/3 chance for Gun, 1/3 chance for Upgrade
            int randomChoice = Random.Range(0, 4);  // Random value between 0 and 2

            if (randomChoice < 3)  // 2/3 chance for Gun (randomChoice 0 or 1)
            {
                currentSellableType = SellableType.Gun;
            }
            else  // 1/3 chance for Upgrade (randomChoice 2)
            {
                currentSellableType = SellableType.Upgrade;
            }
        }

        if (currentSellableType == SellableType.Gun)
        {
            gunPointer = (Random.Range(0, shopManager.ShopStock.Count));
        }
        else if (currentSellableType == SellableType.Upgrade)
        {
            upgradePointer = (Random.Range(0, shopManager.UpgradeStock.Count));
            RandomiseUpgradeValues();
        }

        SetPrice();
    }

    private void SetPrice()
    {
        if (currentSellableType == SellableType.Upgrade)
        {
            generalCost = shopManager.UpgradeStock[upgradePointer].upgradeCost * shopManager.UpgradeStock[upgradePointer].upgradeAmount;
        }
        else if (currentSellableType == SellableType.Gun)
        {
            generalCost = shopManager.ShopStock[gunPointer].gunCost;
        }
    }
}