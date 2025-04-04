using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<ShopItem> ShopStock = new List<ShopItem>();
    public List<ShopUpgradeItem> UpgradeStock = new List<ShopUpgradeItem>();

    public void RemoveGun(int gunIndex)
    {
        if (gunIndex >= 0 && gunIndex < ShopStock.Count)
        {
            ShopStock.RemoveAt(gunIndex);
        }
        else
        {
            Debug.LogError("Gun index out of range");
        }
    }
}
