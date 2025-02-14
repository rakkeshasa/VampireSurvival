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

        // PlayerController.instance.activeWeapon.LevelUp();
        UIController.instance.levelUpPanel.SetActive(true);

        // 레벨업시 시간정지
        Time.timeScale = 0f;
        UIController.instance.levelUpButtons[1].UpdateButton(PlayerController.instance.activeWeapon);
    }

}
