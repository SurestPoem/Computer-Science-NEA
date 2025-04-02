using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<ShopItem> ShopStock = new List<ShopItem>();
    public List<ShopUpgradeItem> UpgradeStock = new List<ShopUpgradeItem>();

    public void CloseShop()
    {
        if (GameManager.Instance.selectedGameType == GameManager.GameType.Normal)
        {
        }
        else
        {
            GameManager.Instance.DisableShop();
        }
    }
}
