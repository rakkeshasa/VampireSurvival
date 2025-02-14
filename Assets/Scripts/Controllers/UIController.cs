using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider expSlider;
    public TMP_Text levelText;

    public LevelUpButton[] levelUpButtons;

    public GameObject levelUpPanel;

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
}
