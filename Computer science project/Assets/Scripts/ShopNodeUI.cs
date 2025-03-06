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
    public TextMeshProUGUI shopNodeName;
    public TextMeshProUGUI shopNodeStats;
    public ShopNode shopNode;

    void Update()
    {
        GunIconHandling();
        HandleText();
    }

    public void GunIconHandling()
    {
        // Access the current gun prefab from the ShopNode class
        if (shopNode.ShopStock.Count > 0)  // Ensure there's at least one item in the shop stock
        {
            // Access the current gunPrefab and set its icon
            gunShopImage.sprite = shopNode.ShopStock[shopNode.stockPointerThingy].gunPrefab.GetComponent<Gun>().gunIcon;
        }
        else
        {
            // Handle case where there is no item to display
            gunShopImage.sprite = noGunImageLol;
        }
    }




    public void HandleText()
    {
        costText.text = (shopNode.ShopStock[shopNode.stockPointerThingy].gunCost.ToString() + " BUY");
        shopNodeStats.text =
            "Damage: " + shopNode.ShopStock[shopNode.stockPointerThingy].gunPrefab.GetComponent<Gun>().damageStat + "\n" +
            "Cooldown: " + shopNode.ShopStock[shopNode.stockPointerThingy].gunPrefab.GetComponent<Gun>().cooldownTime + "\n" +
            "Bullet Speed: " + shopNode.ShopStock[shopNode.stockPointerThingy].gunPrefab.GetComponent<Gun>().bulletSpeed;
        shopNodeName.text = (shopNode.ShopStock[shopNode.stockPointerThingy].gunPrefab.GetComponent<Gun>().gunName);

    }

}
