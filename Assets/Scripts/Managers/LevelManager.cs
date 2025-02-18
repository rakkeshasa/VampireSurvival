using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private bool gameActive;
    public float timer;
    public float waitEndScreen = 1f;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gameActive = true;
    }

    void Update()
    {
        if(gameActive)
        {
            timer += Time.deltaTime;
            UIController.instance.UpdateTimer(timer);
        }
    }

    public void EndLevel()
    {
        gameActive = false;

        StartCoroutine(EndLevelCor());
    }

    IEnumerator EndLevelCor()
    {
        yield return new WaitForSeconds(waitEndScreen); // 1초 후 리턴

        float minutes = Mathf.FloorToInt(timer / 60f);
        float seconds = Mathf.FloorToInt(timer % 60f);

        UIController.instance.endTimeText.text = minutes.ToString() + " : " + seconds.ToString("00");
        UIController.instance.levelEndPanel.SetActive(true);
    }
}
