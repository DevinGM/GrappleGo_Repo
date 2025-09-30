using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/25/2024
/// handles game manager
/// holds score
/// holds temp ui
/// </summary>

public class GameManager : Singleton<GameManager>
{
    // player score
    public int distanceScore = 0;
    public int pickupsScore = 0;
    // highest score ever gained
    public int highScore = 0;

    // Update is called once per frame
    void Update()
    {
        // manage high score
        if (highScore < (distanceScore + pickupsScore))
            highScore = distanceScore + pickupsScore;
    }

    // temp prototyping ui
    private void OnGUI()
    {
        GUIStyle customStyle = new GUIStyle(GUI.skin.label);
        customStyle.fontSize = 30;

        Rect scoreText = new Rect(30, 30, 300, 40); // x, y, width, height
        GUI.Label(scoreText, "Score: " + (distanceScore + pickupsScore), customStyle);
        Rect highScoreText = new Rect(30, 90, 300, 40); // x, y, width, height
        GUI.Label(highScoreText, "High Score: " + highScore, customStyle);
    }
}