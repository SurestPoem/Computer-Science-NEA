using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopUpgradeItem
{
    public enum UpgradeType {MaxHealth, MoveSpeed, RegenRate};
    public UpgradeType upgradeType;
    public int upgradeAmount;   // The amount by which the upgrade will affect the stat
    public int upgradeCost;
    public Sprite upgradeImage;

}