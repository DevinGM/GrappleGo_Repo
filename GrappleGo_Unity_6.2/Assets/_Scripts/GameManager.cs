using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Devin G Monaghan
/// 10/14/2024
/// HANDLES GAME MANAGER
/// holds temp ui
/// holds InRun reference
/// holds saved variables
///     upgradeable stats
///     currency
///     high score
/// </summary>

public class GameManager : Singleton<GameManager>
{
    // is the player in a run?
    public bool InRun { get; private set; } = false;

    // score gained from distance traversed
    public int distanceScore = 0;
    // score gained from pickups like coins
    public int pickupsScore = 0;

    #region Saved Stats

    // highest score ever gained
    public int highScore = 0;
    // player's current amount of currency
    public int currencyAmount = 0;
    // value of coin pickups, defaults to 10
    public int coinValue = 10;
    // speed player climbs at
    public float playerClimbSpeed = 12f;
    // speed grapple climbs at
    public float grappleClimbSpeed = 12f;
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

    #endregion

    #region OnEnable & OnDisable

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, OnRunStart);
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);

        // load saved data
        SaveSystem.Load();
    }

    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunStart, OnRunStart);
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);

        // save data
        SaveSystem.Save();
    }

    #endregion

    #region OnRunStart & OnRunEnd

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
        currencyAmount += (pickupsScore + distanceScore);

        // reset score
        pickupsScore = 0;
        distanceScore = 0;
    }

    #endregion

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

    #region Save & Load
    // save stats to the given struct
    public void Save(ref GameManagerSaveData data)
    {
        data.highScore = highScore;
        data.currency = currencyAmount;
        data.coinValue = coinValue;
        data.playerClimbSpeed = playerClimbSpeed;
        data.grappleClimbSpeed = grappleClimbSpeed;
        data.boostDuration = boostDuration;
        data.dashDuration = dashDuration;
        data.dynamiteDuration = dynamiteDuration;
        data.gunDuration = gunDuration;
        data.purchasedExtraLife = purchasedExtraLife;
        data.purchasedHeadStart = purchasedHeadStart;
    }

    // load stats from the given struct
    public void Load(ref GameManagerSaveData data)
    {
        highScore = data.highScore;
        currencyAmount = data.currency;
        coinValue = data.coinValue;
        playerClimbSpeed = data.playerClimbSpeed;
        grappleClimbSpeed = data.grappleClimbSpeed;
        boostDuration = data.boostDuration;
        dashDuration = data.dashDuration;
        dynamiteDuration = data.dynamiteDuration;
        gunDuration = data.gunDuration;
        purchasedExtraLife = data.purchasedExtraLife;
        purchasedHeadStart = data.purchasedHeadStart;
    }
    #endregion
}

// struct for holding GameManger's save data
[System.Serializable]
public struct GameManagerSaveData
{
    // highest score ever gained
    public int highScore;
    // player's current amount of currency
    public int currency;
    // value of coin pickups, defaults to 10
    public int coinValue;
    // speed player climbs at
    public float playerClimbSpeed;
    // speed grapple climbs at
    public float grappleClimbSpeed;
    // extra boost powerup duration
    public float boostDuration;
    // extra dash powerup duration
    public float dashDuration;
    // extra dynamite powerup duration
    public float dynamiteDuration;
    // extra gun powerup duration
    public float gunDuration;
    // turns on when player purchases extra life upgrade
    public bool purchasedExtraLife;
    // turns on when player purchases headstart upgrade
    public bool purchasedHeadStart;
}