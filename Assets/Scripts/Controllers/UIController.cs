using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider expSlider;
    public TMP_Text levelText;
    public TMP_Text coinText;

    public LevelUpButton[] levelUpButtons;

    public GameObject levelUpPanel;

    public StatUpgradeUI moveSpeedDisplay, healthDisplay, pickupRangeDisplay, maxWeaponsDisplay;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateExp(int currentExp, int levelExp, int currentLevel)
    {
        expSlider.maxValue = levelExp;
        expSlider.value = currentExp;

        // "Level: " + currentLevel;
        levelText.text = $"Level: {currentLevel}";
    }

    public void UpdateCoins()
    {
        coinText.text = "Coins: " + CoinController.instance.currentCoins;
    }

    public void SkipButton()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void PurchaseMoveSpeed()
    {
        PlayerStatController.instance.PurchaseMoveSpeed();
        SkipButton();
    }

    public void PurchaseHealth()
    {
        PlayerStatController.instance.PurchaseHealth();
        SkipButton();
    }

    public void PurchasePickupRange()
    {
        PlayerStatController.instance.PurchasePickupRange();
        SkipButton();
    }

    public void PurchaseMaxWeapons()
    {
        PlayerStatController.instance.PurchaseMaxWeapons();
        SkipButton();
    }
}
