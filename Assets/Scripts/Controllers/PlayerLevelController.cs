using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelController : MonoBehaviour
{
    public static PlayerLevelController instance;

    private int currentEXP;
    public ExpPickup pickup;

    public List<int> expLevels;
    private int currentLevel = 1;
    private int maxLevel = 100;

    public List<Weapon> upgradeWeapons;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        while(expLevels.Count < maxLevel)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }
    }

    void Update()
    {
        
    }

    public void GetExp(int exp)
    {
        currentEXP += exp;
        if(currentEXP >= expLevels[currentLevel])
        {
            LevelUp();
        }

        UIController.instance.UpdateExp(currentEXP, expLevels[currentLevel], currentLevel);
    }

    public void SpawnExp(Vector3 position, int exp)
    {
        Instantiate(pickup, position, Quaternion.identity).expValue = exp;
    }

    private void LevelUp()
    {
        currentEXP -= expLevels[currentLevel];
        currentLevel++;

        // 맥스 레벨시 레벨업x
        if(currentLevel >= expLevels.Count)
        {
            currentLevel = expLevels.Count - 1;
        }

        UIController.instance.levelUpPanel.SetActive(true);

        // 레벨업시 시간정지
        Time.timeScale = 0f;

        upgradeWeapons.Clear();
        List<Weapon> availableWeapons = new List<Weapon>();
        availableWeapons.AddRange(PlayerController.instance.activeWeapons);

        if(availableWeapons.Count > 0)
        {
            int selected = Random.Range(0, availableWeapons.Count);
            upgradeWeapons.Add(availableWeapons[selected]);
            availableWeapons.RemoveAt(selected);
        }

        if(PlayerController.instance.activeWeapons.Count + PlayerController.instance.maxLevelWepons.Count < PlayerController.instance.maxWeapon)
        {
            availableWeapons.AddRange(PlayerController.instance.inactiveWeapons);
        }
        
        for(int i = upgradeWeapons.Count; i < 3; i++)
        {
            if (availableWeapons.Count > 0)
            {
                int selected = Random.Range(0, availableWeapons.Count);
                upgradeWeapons.Add(availableWeapons[selected]);
                availableWeapons.RemoveAt(selected);
            }
        }

        for(int i = 0; i < upgradeWeapons.Count; i++)
        {
            UIController.instance.levelUpButtons[i].UpdateButton(upgradeWeapons[i]);
        }

        for(int i = 0; i < UIController.instance.levelUpButtons.Length; i++)
        {
            if(i < upgradeWeapons.Count)
            {
                UIController.instance.levelUpButtons[i].gameObject.SetActive(true);
            }
            else
            {
                UIController.instance.levelUpButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
