using UnityEngine;
using TMPro;

public class StatUpgradeUI : MonoBehaviour
{
    public TMP_Text valueText, costText;

    public GameObject upgradeButton;

    public void UpdateDisplay(int cost, float oldValue, float newValue)
    {
        valueText.text = "Value: " + oldValue.ToString("F1") + " -> " + newValue.ToString("F1"); // 소수점 1자리까지만 출력
        costText.text = "Cost: " + cost;

        if(cost <= CoinController.instance.currentCoins)
        {
            upgradeButton.SetActive(true);
        }
        else
        {
            upgradeButton.SetActive(false);
        }
    }

    public void ShowMaxLevel()
    {
        valueText.text = "Max Level";
        costText.text = "Max Level";
        upgradeButton.SetActive(false);
    }    
}
