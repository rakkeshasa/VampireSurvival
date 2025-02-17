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
        if(weapon.gameObject.activeSelf)
        {
            upgradeDescText.text = weapon.stats[weapon.weaponLevel].upgradeText;
            weaponIcon.sprite = weapon.icon;

            int weaponLevel = weapon.weaponLevel + 1;
            nameLevelText.text = weapon.name + " - Lvl " + weaponLevel;
        }
        else
        {
            upgradeDescText.text = "Unlock " + weapon.name;
            weaponIcon.sprite = weapon.icon;
            nameLevelText.text = weapon.name;
        }
        assignedWeapon = weapon;

    }

    public void SelectUpgrade()
    {
        if(assignedWeapon != null)
        {
            if(assignedWeapon.gameObject.activeSelf)
            {
                assignedWeapon.LevelUp();
            }
            else
            {
                PlayerController.instance.AddWeapon(assignedWeapon);
            }

            // ���׷��̵�â ����
            UIController.instance.levelUpPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
