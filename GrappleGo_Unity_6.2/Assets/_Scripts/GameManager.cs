using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Devin G Monaghan
/// 11/4/2024
/// HANDLES GAME MANAGER
/// handles world acceleration
/// holds InRun reference
/// holds world movement speed
/// moves to death scene on run end
/// holds saved variables
///     upgradeable stats
///     currency
///     high score
/// </summary>

public class GameManager : Singleton<GameManager>
{
    // is the acceleration cooldown on?
    private bool _accelCooldown = false;
    // length in seconds before world accelerates again
    private float _accelTime = 7.5f;
    // beginning world speed
    private float _startSpeed = 7f;
    // amount world speed accelerates at a time
    private float _accelSpeed = .5f;

    // is the player in a run?
    public bool InRun { get; private set; } = false;

    // score gained from distance traversed
    public int distanceScore = 0;
    // score gained from pickups like coins
    public int pickupsScore = 0;
    // score of most recent run
    public int lastScore = 0;
    // speed world is currently moving at
    public float currentMoveSpeed;

    ////////////////////////////////////////////////////////////////////////////
    [Header("Stats that get saved")]

    // speed player moves at
    public float playerMoveSpeed = 7f;
    // game volume on scale from 0.0001 to 1, defaults to .7
    public float volume = .7f;
    // has the player been given the powerup tutorials?
    public bool playedDashTutorial = false;
    public bool playedDynamiteTutorial = false;

    #region Score/Currency

    // highest score ever gained
    public int highScore = 0;
    // player's current amount of currency
    public int currencyAmount = 0;
    // value of coin pickups, defaults to 10
    public int coinValue = 10;

    #endregion

    #region Powerup Variables

    // extra boost powerup duration
    public float boostDuration = 0f;
    // extra gun powerup duration
    public float gunDuration = 0f;
    // number of possible held dash charges, defaults to 3
    public int maxDashCharges = 3;
    // number of possible held dynamite charges, defaults to 3
    public int maxDynamiteCharges = 3;
    // turns on when player purchases extra life upgrade
    public bool purchasedExtraLife = false;
    // turns on when player purchases headstart upgrade
    public bool purchasedHeadStart = false;

    #endregion

    /// Functions

    #region OnEnable, OnDisable, & OnApplicationQuit

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, OnRunStart);
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);

        // load saved data
        SaveSystem.Load();
        //print("GameManager loaded in OnEnable");
    }

    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunStart, OnRunStart);
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);

        // save data
        SaveSystem.Save();
        //print("GameManager saved in OnDisable");
    }

    // save on application quit
    private void OnApplicationQuit()
    {
        // save data
        SaveSystem.Save();
        //print("GameManager saved in OnApplicationQuit");
    }

    #endregion

    #region OnRunStart & OnRunEnd

    // called when run starts
    private void OnRunStart()
    {
        InRun = true;
        currentMoveSpeed = _startSpeed;

        // save data
        SaveSystem.Save();
        //print("GameManager saved in OnRunStart");
    }

    // called when run ends
    private void OnRunEnd()
    {
        InRun = false;

        // add to currency
        currencyAmount += (pickupsScore + distanceScore);
        // set last score
        lastScore = (pickupsScore + distanceScore);

        // reset score
        pickupsScore = 0;
        distanceScore = 0;

        // save data
        SaveSystem.Save();
        //print("GameManager saved in on run end");

        // wait one frame to make sure data is saved beofer moving to death scene
        StartCoroutine(MoveToDeathScene());
    }

    #endregion

    // wait _accelTime amount of seconds before increasing speed by _accelSpeed
    private IEnumerator Accelerate()
    {
        _accelCooldown = true;
        yield return new WaitForSeconds(_accelTime);
        currentMoveSpeed += _accelSpeed;
        _accelCooldown = false;
    }

    // move to scene at given build index
    //      0 = Main Menu, 1 = Level, 2 = Death Menu, 3 = Shop
    private void MoveToScene(int index)
    {
        SceneManager.LoadScene(index);
        print("scene changed");
    }

    // wait one frame before moving to death scene
    private IEnumerator MoveToDeathScene()
    {
        yield return null;
        MoveToScene(2);
    }

    // Update is called once per frame
    void Update()
    {
        // manage high score
        if (highScore < (distanceScore + pickupsScore))
            highScore = distanceScore + pickupsScore;

        // only do logic during run
        if (InRun)
        {
            // increase speed by 1 every 5 seconds
            if (!_accelCooldown)
                StartCoroutine(Accelerate());
        }
    }

    #region Save & Load

    // save stats to the given struct
    public void Save(ref GameManagerSaveData data)
    {
        data.highScore = highScore;
        data.currency = currencyAmount;
        data.coinValue = coinValue;
        data.playerClimbSpeed = playerMoveSpeed;
        data.volume = volume;
        data.boostDuration = boostDuration;
        data.gunDuration = gunDuration;
        data.maxDashCharges = maxDashCharges;
        data.maxDynamiteCharges = maxDynamiteCharges;
        data.purchasedExtraLife = purchasedExtraLife;
        data.purchasedHeadStart = purchasedHeadStart;
        data.playedDashTutorial = playedDashTutorial;
        data.playedDynamiteTutorial = playedDynamiteTutorial;
    }

    // load stats from the given struct
    public void Load(ref GameManagerSaveData data)
    {
        highScore = data.highScore;
        currencyAmount = data.currency;
        coinValue = data.coinValue;
        playerMoveSpeed = data.playerClimbSpeed;
        volume = data.volume;
        boostDuration = data.boostDuration;
        gunDuration = data.gunDuration;
        maxDashCharges = data.maxDashCharges;
        maxDynamiteCharges = data.maxDynamiteCharges;
        purchasedExtraLife = data.purchasedExtraLife;
        purchasedHeadStart = data.purchasedHeadStart;
        playedDashTutorial = data.playedDashTutorial;
        playedDynamiteTutorial = data.playedDynamiteTutorial;
    }

    // resets variables to default values
    public void ResetData(ref GameManagerSaveData data)
    {
        highScore = 0;
        currencyAmount = 0;
        coinValue = 10;
        playerMoveSpeed = 7f;
        volume = .7f;
        boostDuration = 0f;
        gunDuration = 0f;
        maxDashCharges = 3;
        maxDynamiteCharges = 3;
        purchasedExtraLife = false;
        purchasedHeadStart = false;
        playedDashTutorial = false;
        playedDynamiteTutorial = false;
    }

    #endregion

}

#region Save Data

// struct for holding GameManager's save data
[System.Serializable]
public struct GameManagerSaveData
{
    // highest score ever gained
    public int highScore;
    // player's current amount of currency
    public int currency;
    // value of coin pickups, defaults to 10
    public int coinValue;
    // game volume on scale from 0.0001 to 1, defaults to .7
    public float volume;
    // speed player climbs at
    public float playerClimbSpeed;
    // extra boost powerup duration
    public float boostDuration;
    // extra gun powerup duration
    public float gunDuration;
    // number of possible held dynamite charges, defaults to 3
    public int maxDynamiteCharges;
    // number of possible held dash charges, defaults to 3
    public int maxDashCharges;
    // turns on when player purchases extra life upgrade
    public bool purchasedExtraLife;
    // turns on when player purchases headstart upgrade
    public bool purchasedHeadStart;
    // has the player been given the powerup tutorials?
    public bool playedDashTutorial;
    public bool playedDynamiteTutorial;
}

#endregion