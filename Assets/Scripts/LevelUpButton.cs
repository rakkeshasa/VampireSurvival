using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpButton : MonoBehaviour
{
    public TMP_Text upgradeDescText, nameLevelText;
    public Image weaponIcon;

    private Weapon assignedWeapon;

    public void UpdateButton(Weapon weapon)
    {
        upgradeDescText.text = weapon.stats[weapon.weaponLevel].upgradeText;
        weaponIcon.sprite = weapon.icon;

        int weaponLevel = weapon.weaponLevel + 1;
        nameLevelText.text = weapon.name + " - Lvl " + weaponLevel;
        assignedWeapon = weapon;

    }

    public void SelectUpgrade()
    {
        if(assignedWeapon != null)
        {
            assignedWeapon.LevelUp();

            // 업그레이드창 종료
            UIController.instance.levelUpPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
