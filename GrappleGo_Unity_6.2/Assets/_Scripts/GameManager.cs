using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/30/2024
/// handles game manager
/// holds score
/// holds temp ui
/// holds game state bools:
///     InRun
/// </summary>

public class GameManager : Singleton<GameManager>
{
    // score gained from distance traversed
    public int distanceScore = 0;
    // score gained from pickups like coins
    public int pickupsScore = 0;
    // highest score ever gained
    public int highScore = 0;

    // is the player in a run?
    public bool InRun { get; private set; } = false;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, OnRunStart);
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);
    }

    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunStart, OnRunStart);
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);
    }

    // called when run starts
    private void OnRunStart()
    {
        InRun = true;
    }

    // called when run ends
    private void OnRunEnd()
    {
        InRun = false;
    }

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
        customStyle.fontSize = 60;

        Rect scoreText = new Rect(30, 30, 600, 80); // x, y, width, height
        GUI.Label(scoreText, "Score: " + (distanceScore + pickupsScore), customStyle);
        Rect highScoreText = new Rect(30, 120, 600, 80); // x, y, width, height
        GUI.Label(highScoreText, "High Score: " + highScore, customStyle);
    }
}