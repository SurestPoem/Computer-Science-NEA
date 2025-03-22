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
        PickSellableType();
    }


    private void RemoveSellable()
    {
        shopManager.ShopStock.RemoveAt(gunPointer);
    }

    public void BuyGun()
    {
        if (player.currentCurrency >= generalCost && player.level >= shopManager.ShopStock[gunPointer].levelRequired)
        {
            player.AddGun(shopManager.ShopStock[gunPointer].gunPrefab);
            RemoveSellable();
            player.UseCurrency(generalCost);
            currentSellableType = SellableType.Null;
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
            currentSellableType = (Random.Range(0, 2) == 0) ? SellableType.Gun : SellableType.Upgrade;
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