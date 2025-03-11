using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopUpgradeItem
{
    public enum upgradeType {MaxHealth, MoveSpeed, RegenRate};  // The type of upgrade, such as 'Damage', 'Cooldown', etc.
    public int upgradeAmount;   // The amount by which the upgrade will affect the stat
    public int upgradeCost;            // The cost of this upgrade
}