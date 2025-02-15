using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<WeaponStats> stats;
    public int weaponLevel;

    [HideInInspector]
    public bool statsUpdated;

    public Sprite icon;

    public void LevelUp()
    {
        if(weaponLevel < stats.Count - 1)
        {
            weaponLevel++;
            statsUpdated = true;
        }
    }
}

[System.Serializable]
public class WeaponStats
{
    public float speed, damage, range, attackInterval, amount, duration;
    public string upgradeText;
}