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
    public int gunPointerThingy = 0;
    public int upgradePointerThingy = 0;
    public ShopManager shopManager;

    void Start()
    {
        PickSellableType();
    }


    private void RemoveSellable()
    {
        shopManager.ShopStock.RemoveAt(gunPointerThingy);
    }

    public void BuyGun()
    {
        if (player.currentCurrency >= generalCost)
        {
            player.AddGun(shopManager.ShopStock[gunPointerThingy].gunPrefab);
            RemoveSellable();
            player.UseCurrency(generalCost);
        }
        else
        {
            Debug.Log("Player is broke, not enough currency");
        }
    }

    public void BuyUpgrade()
    {
        if (player.currentCurrency >= generalCost)
        {
            // Correct way to access the enum value from ShopUpgradeItem
            ShopUpgradeItem.UpgradeType upgradeType = shopManager.UpgradeStock[upgradePointerThingy].upgradeType;

            switch (upgradeType)
            {
                case ShopUpgradeItem.UpgradeType.MaxHealth:
                    player.IncreaseStats("maxHealth", shopManager.UpgradeStock[upgradePointerThingy].upgradeAmount);
                    break;
                case ShopUpgradeItem.UpgradeType.MoveSpeed:
                    player.IncreaseStats("moveSpeed", shopManager.UpgradeStock[upgradePointerThingy].upgradeAmount);
                    break;
                case ShopUpgradeItem.UpgradeType.RegenRate:
                    player.IncreaseStats("regenRate", shopManager.UpgradeStock[upgradePointerThingy].upgradeAmount);
                    break;
                default:
                    Debug.LogError("Unknown upgrade type.");
                    return;
            }
            player.UseCurrency(generalCost);
        }
        else
        {
            Debug.Log("Player is broke, not enough currency");
        }
    }


    public void RandomiseUpgradeValues()
    {
        // Correctly access the upgradeType field of the selected ShopUpgradeItem
        ShopUpgradeItem.UpgradeType upgradeType = shopManager.UpgradeStock[upgradePointerThingy].upgradeType;

        switch (upgradeType)
        {
            case ShopUpgradeItem.UpgradeType.MaxHealth:
                // Randomize the amount of max health the player can buy (between 1 and 5, for example)
                shopManager.UpgradeStock[upgradePointerThingy].upgradeAmount = Random.Range(1, 7);
                break;

            case ShopUpgradeItem.UpgradeType.MoveSpeed:
                // Randomize move speed upgrade (between 1 and 3, for example)
                shopManager.UpgradeStock[upgradePointerThingy].upgradeAmount = Random.Range(1, 4);
                break;

            case ShopUpgradeItem.UpgradeType.RegenRate:
                // Randomize regen rate upgrade (between 1 and 2, for example)
                shopManager.UpgradeStock[upgradePointerThingy].upgradeAmount = Random.Range(1, 4);
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
            currentSellableType = (Random.Range(0, 2) == 0) ? SellableType.Gun : SellableType.Upgrade;
        }

        if (currentSellableType == SellableType.Gun)
        {
            gunPointerThingy = (Random.Range(0, shopManager.ShopStock.Count));
        }
        else if (currentSellableType == SellableType.Upgrade)
        {
            upgradePointerThingy = (Random.Range(0, shopManager.UpgradeStock.Count));
            RandomiseUpgradeValues();
        }

        SetPrice();
    }

    private void SetPrice()
    {
        if (currentSellableType == SellableType.Upgrade)
        {
            generalCost = shopManager.UpgradeStock[upgradePointerThingy].upgradeCost * shopManager.UpgradeStock[upgradePointerThingy].upgradeAmount;
        }
        else if (currentSellableType == SellableType.Gun)
        {
            generalCost = shopManager.ShopStock[gunPointerThingy].gunCost;
        }
    }
}