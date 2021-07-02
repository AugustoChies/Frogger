using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public static HudController Instance;

    public Text livesText;
    public Text timeText;
    public Text scoreText;

    public GameObject YouLoseCanvas;
    public GameObject YouWinCanvas;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateLives()
    {
        livesText.text = "Lives: " + LaneManager.Instance.lives;
        if (LaneManager.Instance.lives < 0)
        {
            print("You've Died");
            NetworkInfoManager.Instance.DisablePlayers();
            YouLoseCanvas.gameObject.SetActive(true);
        }
    }

    public void UpdateTime(float currentTime)
    {
        timeText.text = "Remaining Time: " + currentTime.ToString("F1") + "s";
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + LaneManager.Instance.score;
    }

    public void EndGameOutOfTime()
    {
        print("You've Died");
        NetworkInfoManager.Instance.DisablePlayers();
        YouLoseCanvas.gameObject.SetActive(true);
    }
}
