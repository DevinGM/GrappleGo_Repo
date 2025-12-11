using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Devi G Monaghan
/// 12/11/2025
/// Handles shop behaviors ¢¢
/// Holds shop save data for prices
/// </summary>

public class Shop : MonoBehaviour
{
    // static reference to shop to allow save system to access it's save & load
    public static Shop Instance;

    #region References

    [Header("Buttons")]
    // references to buttons
    [SerializeField] private UnityEngine.UI.Button _coinButton;
    [SerializeField] private UnityEngine.UI.Button _playerMoveSpeedButton;
    [SerializeField] private UnityEngine.UI.Button _boostDurationButton;
    [SerializeField] private UnityEngine.UI.Button _dashChargesButton;
    [SerializeField] private UnityEngine.UI.Button _gunDurationButton;
    [SerializeField] private UnityEngine.UI.Button _extraLifeButton;
    [SerializeField] private UnityEngine.UI.Button _dynamiteChargesButton;
    [SerializeField] private UnityEngine.UI.Button _headStartButton;
    [Header("Text Boxes")]
    // references to texts
    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private TMP_Text _playerMoveSpeedText;
    [SerializeField] private TMP_Text _boostDurationText;
    [SerializeField] private TMP_Text _dashChargesText;
    [SerializeField] private TMP_Text _gunDurationText;
    [SerializeField] private TMP_Text _dynamiteChargesText;
    [SerializeField] private TMP_Text _extraLifeText;
    [SerializeField] private TMP_Text _headStartText;
    [SerializeField] private TMP_Text _currencyText;

    #endregion

    #region Stats

    [Header("Upgrade Values")]
    /// amount stat increases per upgrade
    [SerializeField] private int _coinValue = 10;
    [SerializeField] private float _playerMoveSpeedValue = 1f;
    [SerializeField] private float _boostDurationValue = 1f;
    [SerializeField] private float _gunDurationValue = 1f;
    // maximum player movement speed upgrade value
    [SerializeField] private float _maxSpeedValue = 40f;

    /// prices of upgrades
    [Header("Upgrade Prices")]
    [SerializeField] private int _coinPrice = 1000;
    [SerializeField] private int _playerMoveSpeedPrice = 1000;
    [SerializeField] private int _boostDurationPrice = 1000;
    [SerializeField] private int _gunDurationPrice = 1000;
    [SerializeField] private int _dashChargesPrice = 10000;
    [SerializeField] private int _dynamiteChargesPrice = 10000;
    [SerializeField] private int _extraLifePrice = 1000;
    [SerializeField] private int _headStartPrice = 1000;
    // how much prices increase per purchase
    [Header("How much prices\nincrease per purchase")]
    [SerializeField] private int _priceIncrease = 1000;

    #endregion

    #region OnEnable & OnDisable

    void OnEnable()
    {
        // set static reference
        Instance = this;
        // load saved data
        SaveSystem.Load();
        SetButtons();
    }

    private void OnDisable()
    {
        // save data
        SaveSystem.Save();
        // reset Instance just in case
        Instance = null;
    }

#endregion

    // set the text of all the upgrade buttons and currency display
    // if player doesn't have enough currency to purchase an upgrade, turn that button off
    public void SetButtons()
    {
        int currencyAmount = GameManager.Instance.currencyAmount;

        // coin value upgrade
        _coinText.text = "Coin Value\n¢" + _coinPrice + "\n" + GameManager.Instance.coinValue + " > " 
            + (GameManager.Instance.coinValue + _coinValue);
        if (currencyAmount < _coinPrice)
            _coinButton.interactable = false;

        // player move speed upgrade
        // normal logic, do if player move speed is below cap
        if (GameManager.Instance.playerMoveSpeed < _maxSpeedValue)
        {
            _playerMoveSpeedText.text = "Movement Speed\n¢" + _playerMoveSpeedPrice + "\n" + GameManager.Instance.playerMoveSpeed + " > "
                + (GameManager.Instance.playerMoveSpeed + _playerMoveSpeedValue);
            if (currencyAmount < _playerMoveSpeedPrice)
                _playerMoveSpeedButton.interactable = false;
        }
        // if player move speed hits cap, turn off button
        else
        {
            _playerMoveSpeedText.text = "Movement Speed\nMaxxed\n" + _maxSpeedValue;
            _playerMoveSpeedButton.interactable = false;
        }

        // boost duration upgrade
        _boostDurationText.text = "Boost Duration\n¢" + _boostDurationPrice + "\n" + GameManager.Instance.boostDuration + " > "
            + (GameManager.Instance.boostDuration + _boostDurationValue);
        if (currencyAmount < _boostDurationPrice)
            _boostDurationButton.interactable = false;

        // gun duration upgrade
        _gunDurationText.text = "Gun Duration\n¢" + _gunDurationPrice + "\n" + GameManager.Instance.gunDuration + " > "
            + (GameManager.Instance.gunDuration + _gunDurationValue);
        if (currencyAmount < _gunDurationPrice)
            _gunDurationButton.interactable = false;

        /// extra life
        if (!GameManager.Instance.purchasedExtraLife)
        {
            _extraLifeText.text = "Extra Life\n¢" + _extraLifePrice;
            if (currencyAmount < _extraLifePrice)
                _extraLifeButton.interactable = false;
        }
        // if player has purchased extra life, turn off button and change text to purchased
        else
        {
            _extraLifeButton.interactable = false;
            _extraLifeText.text = "Extra Life\n[Purchased]";
        }

        /// head start
        if (!GameManager.Instance.purchasedHeadStart)
        {
            _headStartText.text = "Head Start\n¢" + _headStartPrice;
            if (currencyAmount < _headStartPrice)
                _headStartButton.interactable = false;
        }
        // if player has purchased head start, turn off button and change text to purchased
        else
        {
            _headStartButton.interactable = false;
            _headStartText.text = "Head Start\n[Purchased]";
        }

        /// dash charges
        if (GameManager.Instance.maxDashCharges < 5)
        {
            int currentCharges = GameManager.Instance.maxDashCharges;
            int uppedCharges = currentCharges + 1;
            _dashChargesText.text = "Dash Charges\n¢" + _dashChargesPrice + "\n" + currentCharges + " > " + uppedCharges;
            if (currencyAmount < _dashChargesPrice)
                _dashChargesButton.interactable = false;
        }
        // if player has maxxed dash charges, turn button off and change text, defaults to max of 5
        else
        {
            _dashChargesButton.interactable = false;
            _dashChargesText.text = "Dash Charges\n[Maxxed]\n5";
        }

        /// dynamite charges
        if (GameManager.Instance.maxDynamiteCharges < 5)
        {
            int currentCharges = GameManager.Instance.maxDynamiteCharges;
            int uppedCharges = currentCharges + 1;
            _dynamiteChargesText.text = "Dynamite Charges\n¢" + _dynamiteChargesPrice + "\n" + currentCharges + " > " + uppedCharges;
            if (currencyAmount < _dynamiteChargesPrice)
                _dynamiteChargesButton.interactable = false;
        }
        // if player has maxxed dynamite charges, turn button off and change text, defaults to max of 5
        else
        {
            _dynamiteChargesButton.interactable = false;
            _dynamiteChargesText.text = "Dynamite Charges\n[Maxxed]\n5";
        }

        // currency display
        if (_currencyText != null)
            _currencyText.text = "¢" + currencyAmount;
    }

    #region Button Press Functions
    /// Applies corresponding upgrade and updates UI
    public void UpgradeCoinValue()
    {
        GameManager.Instance.coinValue += _coinValue;
        GameManager.Instance.currencyAmount -= _coinPrice;
        _coinPrice += _priceIncrease;
        SetButtons();
    }
    public void UpgradePlayerClimbSpeed()
    {
        GameManager.Instance.playerMoveSpeed += _playerMoveSpeedValue;
        GameManager.Instance.currencyAmount -= _playerMoveSpeedPrice;
        _playerMoveSpeedPrice += _priceIncrease;
        SetButtons();
    }
    public void UpgradeBoostDuration()
    {
        GameManager.Instance.boostDuration += _boostDurationValue;
        GameManager.Instance.currencyAmount -= _boostDurationPrice;
        _boostDurationPrice += _priceIncrease;
        SetButtons();
    }
    public void UpgradeGunDuration()
    {
        GameManager.Instance.gunDuration += _gunDurationValue;
        GameManager.Instance.currencyAmount -= _gunDurationPrice;
        _gunDurationPrice += _priceIncrease;
        SetButtons();
    }
    public void UpgradeExtraLife()
    {
        GameManager.Instance.purchasedExtraLife = true;
        GameManager.Instance.currencyAmount -= _extraLifePrice;
        _extraLifePrice += _priceIncrease;
        SetButtons();
    }
    public void UpgradeHeadStart()
    {
        GameManager.Instance.purchasedHeadStart = true;
        GameManager.Instance.currencyAmount -= _headStartPrice;
        _headStartPrice += _priceIncrease;
        SetButtons();
    }
    public void UpgradeDashCharges()
    {
        GameManager.Instance.maxDashCharges++;
        GameManager.Instance.currencyAmount -= _dashChargesPrice;
        _dashChargesPrice += _priceIncrease;
        SetButtons();
    }
    public void UpgradeDynamiteCharges()
    {
        GameManager.Instance.maxDynamiteCharges++;
        GameManager.Instance.currencyAmount -= _dynamiteChargesPrice;
        _dynamiteChargesPrice += _priceIncrease;
        SetButtons();
    }
    #endregion

    #region Save & Load

    // save stats to the given struct
    public void Save(ref ShopSaveData data)
    {
        data.coinPrice = _coinPrice;
        data.playerMoveSpeedPrice = _playerMoveSpeedPrice;
        data.boostDurationPrice = _boostDurationPrice;
        data.gunDurationPrice = _gunDurationPrice;
        data.dashChargesPrice = _dashChargesPrice;
        data.dynamiteChargesPrice = _dynamiteChargesPrice;
        data.extraLifePrice = _extraLifePrice;
        data.headStartPrice = _headStartPrice;
    }

    // load stats from the given struct
    public void Load(ref ShopSaveData data)
    {
        _coinPrice = data.coinPrice;
        _playerMoveSpeedPrice = data.playerMoveSpeedPrice;
        _boostDurationPrice = data.boostDurationPrice;
        _gunDurationPrice = data.gunDurationPrice;
        _dashChargesPrice = data.dashChargesPrice;
        _dynamiteChargesPrice = data.dynamiteChargesPrice;
        _extraLifePrice = data.extraLifePrice;
        _headStartPrice = data.headStartPrice;
    }


    // resets variables to default values
    public void ResetData(ref ShopSaveData data)
    {
        _coinPrice = 1000;
        _playerMoveSpeedPrice = 1000;
        _boostDurationPrice = 1000;
        _gunDurationPrice = 1000;
        _dashChargesPrice = 1000;
        _dynamiteChargesPrice = 1000;
        _extraLifePrice = 1000;
        _headStartPrice = 1000;
    }

    #endregion

}

#region Save Data

// struct for holding Shop's save data
[System.Serializable]
public struct ShopSaveData
{
    public int coinPrice;
    public int playerMoveSpeedPrice;
    public int boostDurationPrice;
    public int gunDurationPrice;
    public int dashChargesPrice;
    public int dynamiteChargesPrice;
    public int extraLifePrice;
    public int headStartPrice;
}

#endregion