using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopNodeUI : MonoBehaviour
{
    public TextMeshProUGUI costText;
    public Image gunShopImage;
    public Sprite nullIconImage;
    public TextMeshProUGUI shopNodeName;
    public TextMeshProUGUI shopNodeStats;
    public TextMeshProUGUI shopNodeDescription;
    public ShopNode shopNode;
    public ShopManager shopManager;
    public TextMeshProUGUI gunLevelText;

    void Awake()
    {
        shopManager = FindObjectOfType<ShopManager>();
    }
    void Update()
    {
        if (shopNode != null)
        {
            GunIconHandling();
            HandleText();
        }
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
            Sprite gunSprite = shopNode.shopManager.ShopStock[shopNode.gunPointer].gunPrefab.transform.Find("GunVisual").GetComponent<SpriteRenderer>().sprite; // Find the GunVisual child


            // Assign the sprite from the SpriteRenderer to the UI image
            gunShopImage.sprite = gunSprite;
        }
        else if (shopNode.currentSellableType == ShopNode.SellableType.Upgrade)
        {
            gunShopImage.sprite = shopNode.shopManager.UpgradeStock[shopNode.upgradePointer].upgradeImage;
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
                "Damage: " + shopNode.shopManager.ShopStock[shopNode.gunPointer].gunPrefab.GetComponent<Gun>().damageStat + "\n" +
                "Cooldown: " + shopNode.shopManager.ShopStock[shopNode.gunPointer].gunPrefab.GetComponent<Gun>().cooldownTime + "\n" +
                "Bullet Speed: " + shopNode.shopManager.ShopStock[shopNode.gunPointer].gunPrefab.GetComponent<Gun>().bulletSpeed;
            shopNodeName.text = (shopNode.shopManager.ShopStock[shopNode.gunPointer].gunPrefab.GetComponent<Gun>().gunName);
            shopNodeDescription.text = (shopNode.shopManager.ShopStock[shopNode.gunPointer].gunPrefab.GetComponent<Gun>().gunDescription);
            gunLevelText.text = ("Level: " + shopNode.shopManager.ShopStock[shopNode.gunPointer].levelRequired);
        }
        else if (shopNode.currentSellableType == ShopNode.SellableType.Upgrade)
        {
            costText.text = (shopNode.generalCost.ToString() + " Upgrade");
            shopNodeStats.text = ("Amount:" + "\n" + shopNode.shopManager.UpgradeStock[shopNode.upgradePointer].upgradeAmount.ToString());
            shopNodeName.text = shopNode.shopManager.UpgradeStock[shopNode.upgradePointer].upgradeType.ToString();
            shopNodeDescription.text = "A stat upgrade to your " + "\n" + shopNode.shopManager.UpgradeStock[shopNode.upgradePointer].upgradeType.ToString();
            gunLevelText.text = ("Level:  0");
        }
        else if (shopNode.currentSellableType == ShopNode.SellableType.Null)
        {
            costText.text = ("Item purchased");
            shopNodeStats.text = ("Purchased");
            shopNodeName.text = ("Item purchased");
            gunLevelText.text = ("Item purchased");
            shopNodeDescription.text = ("Item purchased");
        }
    }
}