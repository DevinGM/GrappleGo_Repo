using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/17/2024
/// handles game manager
/// holds score
/// holds temp ui
/// </summary>

public class GameManager : Singleton<GameManager>
{
    // player score
    public int score = 0;
    // highest distance ever travelled
    public int highScore = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // manage high score
        if (highScore < score)
            highScore = score;
    }

    // temp prototyping ui
    private void OnGUI()
    {
        GUIStyle customStyle = new GUIStyle(GUI.skin.label);
        customStyle.fontSize = 30;

        Rect scoreText = new Rect(10, 10, 300, 40); // x, y, width, height
        GUI.Label(scoreText, "Score: " + score, customStyle);
        Rect highScoreText = new Rect(10, 50, 300, 40); // x, y, width, height
        GUI.Label(highScoreText, "High Score: " + highScore, customStyle);
    }
}
