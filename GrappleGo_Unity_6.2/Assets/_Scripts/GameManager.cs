using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 10/14/2024
/// handles game manager
/// holds score
/// holds temp ui
/// holds InRun reference
/// holds stats and upgrade values
/// </summary>

public class GameManager : Singleton<GameManager>
{
    // score gained from distance traversed
    public int distanceScore = 0;
    // score gained from pickups like coins
    public int pickupsScore = 0;
    // highest score ever gained
    public int highScore = 0;
    // player's current amount of currency
    public int currency = 0;

    // is the player in a run?
    public bool InRun { get; private set; } = false;

    [Header("Player stats")]
    // value of coin pickups, defaults to 10
    public int coinValue = 10;
    // speed player climbs at
    public float playerClimbSpeed = 12f;
    // speed grapple climbs at
    public float grappleClimbSpeed = 15f;
    // extra boost powerup duration
    public float boostDuration = 0f;
    // extra dash powerup duration
    public float dashDuration = 0f;
    // extra dynamite powerup duration
    public float dynamiteDuration = 0f;
    // extra gun powerup duration
    public float gunDuration = 0f;
    // turns on when player purchases extra life upgrade
    public bool purchasedExtraLife = false;
    // turns on when player purchases headstart upgrade
    public bool purchasedHeadStart = false;

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

        // add to currency
        currency += (pickupsScore + distanceScore);

        // reset score
        pickupsScore = 0;
        distanceScore = 0;
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