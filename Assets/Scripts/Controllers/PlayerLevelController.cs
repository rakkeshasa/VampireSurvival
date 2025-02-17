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

    // 무기 업글 목록
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

        // upgradeWeapons에서 랜덤하게 뽑은 무기를 담아둘 리스트
        List<Weapon> availableWeapons = new List<Weapon>();
        availableWeapons.AddRange(PlayerController.instance.activeWeapons); // 풀업인 무기는 넣지 않음

        // 업그레이드 스킬이 새로운 스킬 획득보다 우선권을 갖도록
        if(availableWeapons.Count > 0)
        {
            // 업그레이드 창이 랜덤하게 나타나도록(a스킬이 무조건 앞으로 온다는 것을 방지)
            int selected = Random.Range(0, availableWeapons.Count);
            upgradeWeapons.Add(availableWeapons[selected]);
            availableWeapons.RemoveAt(selected);
        }

        if(PlayerController.instance.activeWeapons.Count + PlayerController.instance.maxLevelWepons.Count < PlayerController.instance.maxWeapon)
        {
            availableWeapons.AddRange(PlayerController.instance.inactiveWeapons);
        }
        
        // 남는 창에는 새로운 스킬 습득
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

        // 풀업 스킬의 존재로 스킬 업글창이 모두 업뎃이 안된경우 비활성화
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
