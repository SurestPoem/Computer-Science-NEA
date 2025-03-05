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
    public ShopNode shopNode;

    void Update()
    {
        GunIconHandling();
    }

    public void GunIconHandling()
    {
        gunShopImage.sprite = noGunImageLol;
    }
}
