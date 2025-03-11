using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopNodeUI : MonoBehaviour
{
    public TextMeshProUGUI costText;
    public Image gunShopImage;
    public Sprite noGunImageLol;
    public Sprite nullIconImage;
    public TextMeshProUGUI shopNodeName;
    public TextMeshProUGUI shopNodeStats;
    public ShopNode shopNode;
    public ShopManager shopManager;

    void Update()
    {
        GunIconHandling();
        HandleText();
    }

    public void OnBuyButtonPressed()
    {
        if (shopNode.currentSellableType == ShopNode.SellableType.Gun)
        {
            shopNode.BuyGun();
        }
        else if (shopNode.currentSellableType == ShopNode.SellableType.Upgrade)
        {
            shopNode.BuyUpgrade();
        }
        else if (shopNode.currentSellableType == ShopNode.SellableType.Null)
        {
            // No code here
        }
    }

    public void GunIconHandling()
    {
        if (shopNode.currentSellableType == ShopNode.SellableType.Gun)
        {
            gunShopImage.sprite = shopNode.shopManager.ShopStock[shopNode.gunPointerThingy].gunPrefab.GetComponent<Gun>().gunIcon;
        }
        else if (shopNode.currentSellableType == ShopNode.SellableType.Upgrade)
        {
            gunShopImage.sprite = noGunImageLol;
        }
        else if (shopNode.currentSellableType == ShopNode.SellableType.Null)
        {
            gunShopImage.sprite = nullIconImage;
        }
    }

    public void HandleText()
    {
        if (shopNode.currentSellableType == ShopNode.SellableType.Gun)
        {
            costText.text = (shopNode.generalCost.ToString() + " Buy");
            shopNodeStats.text =
                "Damage: " + shopNode.shopManager.ShopStock[shopNode.gunPointerThingy].gunPrefab.GetComponent<Gun>().damageStat + "\n" +
                "Cooldown: " + shopNode.shopManager.ShopStock[shopNode.gunPointerThingy].gunPrefab.GetComponent<Gun>().cooldownTime + "\n" +
                "Bullet Speed: " + shopNode.shopManager.ShopStock[shopNode.gunPointerThingy].gunPrefab.GetComponent<Gun>().bulletSpeed;
            shopNodeName.text = (shopNode.shopManager.ShopStock[shopNode.gunPointerThingy].gunPrefab.GetComponent<Gun>().gunName);
        }
        else if (shopNode.currentSellableType == ShopNode.SellableType.Upgrade)
        {
            costText.text = (shopNode.generalCost.ToString() + " Upgrade");
            shopNodeStats.text = ("Amount:" + "\n" + shopNode.shopManager.UpgradeStock[shopNode.upgradePointerThingy].upgradeAmount.ToString());
            shopNodeName.text = shopNode.shopManager.UpgradeStock[shopNode.upgradePointerThingy].upgradeType.ToString();
        }
        else if (shopNode.currentSellableType == ShopNode.SellableType.Null)
        {
            costText.text = ("Item purchased");
            shopNodeStats.text = ("Purchased");
            shopNodeName.text = ("Item purchased");
        }
    }
}