using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText, coinsText, timeText;

    int score, coins, oldTime;

    public int timeLimit;

    public GameObject pauseScreen, levelEndScreen;

    public static UIManager Instance;
    void Awake() => Instance = this;

    void Start()
    {
        score = coins = oldTime = 0;
        scoreText.text = "SCORE\n0";
        coinsText.text = "COIN\n0";
        timeText.text = "TIME\n" + timeLimit;
    }

    void Update()
    {
        if (oldTime < Mathf.Floor(Time.time))
        {
            oldTime = (int)Mathf.Floor(Time.time);
            timeLimit--;
            timeText.text = "TIME\n" + timeLimit;
            if (timeLimit == 0)
                GameManager.Instance.Retry();
        }
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "SCORE\n" + score.ToString();
    }

    public void AddCoin()
    {
        coins++;
        coinsText.text = "COIN\n" + coins.ToString();
    }

    public GameObject GetPauseScreen() => pauseScreen;
    public GameObject GetEndLevelScreen() => levelEndScreen;
}
