using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public static HudController Instance;

    public Text livesText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateLives()
    {
        livesText.text = "Lives: " + LaneManager.Instance.lives;
    }
}
