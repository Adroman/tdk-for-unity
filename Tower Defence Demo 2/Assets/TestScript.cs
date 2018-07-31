using System;
using System.Collections;
using System.Collections.Generic;
using Scrips.Instances;
using Scrips.Towers.BaseData;
using Scrips.Variables;
using Scrips.Waves;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public IntVariable Lives;

    public IntVariable Gold;

    public IntVariable Wave;

    public IntEnemyStat Profit;

    public TowerInstance Tower;

    public void TestAvailableUpgrades()
    {
        TowerUpgradeLineNode first = null;
        
        foreach (var upgrade in Tower.GetPossibleUpgrades())
        {
            first = upgrade;
            Debug.Log(upgrade.name + " available");
        }
        
        if (first != null) Tower.Upgrade(first);
    }
}
