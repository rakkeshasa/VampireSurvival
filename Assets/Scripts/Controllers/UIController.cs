using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider expSlider;
    public TMP_Text levelText;
    public TMP_Text coinText;
    public TMP_Text timeText;
    public TMP_Text endTimeText;

    public LevelUpButton[] levelUpButtons;

    public GameObject levelUpPanel;
    public GameObject levelEndPanel;
    public GameObject pausePanel;

    public StatUpgradeUI moveSpeedDisplay, healthDisplay, pickupRangeDisplay, maxWeaponsDisplay;

    public string mainMenu;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
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

    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60f);
        float seconds = Mathf.FloorToInt(time % 60f);

        timeText.text = "Time:  " + minutes + ":" + seconds.ToString("00");
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

    public void MainMenuButton()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if(!pausePanel.activeSelf)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
            
        else
        {
            pausePanel.SetActive(false);
            if (!levelUpPanel.activeSelf)
            {
                Time.timeScale = 1f;
            }
        }
    }
}
