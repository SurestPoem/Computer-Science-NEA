using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopNode : MonoBehaviour
{
    public Player player;
    public enum SellableType { Gun, Upgrade }
    public enum UpgradeType { MaxHealth, MoveSpeed, RegenRate };
    public int upgradeCost;
    public int upgradeAmount; //How much is upgraded
    public List<ShopItem> ShopStock = new List<ShopItem>();
    public int stockPointerThingy = 0;


    private void RemoveSellable()
    {
        ShopStock.RemoveAt(stockPointerThingy);
    }

    public void BuyGun()
    {
        if (player.currentCurrency >= ShopStock[stockPointerThingy].gunCost)
        {
            player.AddGun(ShopStock[stockPointerThingy].gunPrefab);
            RemoveSellable();
            player.UseCurrency(ShopStock[stockPointerThingy].gunCost);
        }
        else
        {
            Debug.Log("Player is broke, not enough currency");
        }
    }

    public void BuyUpgrade(UpgradeType upgradeType)
    {
        if (player.currentCurrency >= upgradeCost)
        {
            switch (upgradeType)
            {
                case UpgradeType.MaxHealth:
                    player.IncreaseStats("maxHealth", upgradeAmount);
                    break;
                case UpgradeType.MoveSpeed:
                    player.IncreaseStats("moveSpeed", upgradeAmount);                    
                    break;
                case UpgradeType.RegenRate:
                    player.IncreaseStats("regenRate", upgradeAmount);                    
                    break;
                default:
                    Debug.LogError("Unknown upgrade type.");
                    return;
            }
            player.UseCurrency(upgradeCost);
        }
        else
        {
            Debug.Log("Player is broke, not enough currency");
        }
        }
    }
