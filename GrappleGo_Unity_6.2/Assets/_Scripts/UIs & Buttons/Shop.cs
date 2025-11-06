using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Devi G Monaghan
/// 10/17/2025
/// Handles shop behaviors ¢¢
/// </summary>

public class Shop : MonoBehaviour
{
    [Header("Buttons")]
    // references to buttons
    [SerializeField] private UnityEngine.UI.Button _coinButton;
    [SerializeField] private UnityEngine.UI.Button _playerMoveSpeedButton;
    [SerializeField] private UnityEngine.UI.Button _boostDurationButton;
    [SerializeField] private UnityEngine.UI.Button _dashDurationButton;
    [SerializeField] private UnityEngine.UI.Button _dynamiteDurationButton;
    [SerializeField] private UnityEngine.UI.Button _gunDurationButton;
    [SerializeField] private UnityEngine.UI.Button _extraLifeButton;
    [SerializeField] private UnityEngine.UI.Button _headStartButton;
    [Header("Text Boxes")]
    // references to texts
    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private TMP_Text _playerMoveSpeedText;
    [SerializeField] private TMP_Text _boostDurationText;
    [SerializeField] private TMP_Text _dashDurationText;
    [SerializeField] private TMP_Text _dynamiteDurationText;
    [SerializeField] private TMP_Text _gunDurationText;
    [SerializeField] private TMP_Text _extraLifeText;
    [SerializeField] private TMP_Text _headStartText;

    [Header("Upgrade Values")]
    /// amount stat increases per upgrade
    [SerializeField] private int _coinValue = 10;
    [SerializeField] private float _playerMoveSpeedValue = 1f;
    [SerializeField] private float _boostDurationValue = 1f;
    [SerializeField] private float _dashDurationValue = 1f;
    [SerializeField] private float _dynamiteDurationValue = 1f;
    [SerializeField] private float _gunDurationValue = 1f;
    /// prices of upgrades
    [Header("Upgrade Prices")]
    [SerializeField] private int _coinPrice = 1000;
    [SerializeField] private int _playerMoveSpeedPrice = 1000;
    [SerializeField] private int _boostDurationPrice = 1000;
    [SerializeField] private int _dashDurationPrice = 1000;
    [SerializeField] private int _dynamiteDurationPrice = 1000;
    [SerializeField] private int _gunDurationPrice = 1000;
    [SerializeField] private int _extraLifePrice = 1000;
    [SerializeField] private int _headStartPrice = 1000;

    void OnEnable()
    {
        SetButtons();
        // if player has purchased the extra life or head start upgrades, turn those buttons off
        if (GameManager.Instance.purchasedExtraLife)
        {
            _extraLifeButton.interactable = false;
            _extraLifeText.text = "Extra Life\n[Purchased]";
        }
        if (GameManager.Instance.purchasedHeadStart)
        {
            _headStartButton.interactable = false;
            _headStartText.text = "Head Start\n[Purchased]";
        }
    }

    // set the text of all the upgrade buttons
    // if player doesn't have enough currency to purchase an upgrade, turn that button off
    public void SetButtons()
    {
        // coin value upgrade
        _coinText.text = "Coin Value\n¢" + _coinPrice + "\n" + GameManager.Instance.coinValue + " > " 
            + (GameManager.Instance.coinValue + _coinValue);
        if (GameManager.Instance.currencyAmount < _coinPrice)
            _coinButton.interactable = false;

        // player move speed upgrade
        _playerMoveSpeedText.text = "Movement Speed\n¢" + _playerMoveSpeedPrice + "\n" + GameManager.Instance.playerMoveSpeed + " > "
            + (GameManager.Instance.playerMoveSpeed + _playerMoveSpeedValue);
        if (GameManager.Instance.currencyAmount < _playerMoveSpeedPrice)
            _playerMoveSpeedButton.interactable = false;

        // boost duration upgrade
        _boostDurationText.text = "Boost Duration\n¢" + _boostDurationPrice + "\n" + GameManager.Instance.boostDuration + " > "
            + (GameManager.Instance.boostDuration + _boostDurationValue);
        if (GameManager.Instance.currencyAmount < _boostDurationPrice)
            _boostDurationButton.interactable = false;

        // dash duration upgrade
        _dashDurationText.text = "Dash Duration\n¢" + _dashDurationPrice + "\n" + GameManager.Instance.dashDuration + " > "
            + (GameManager.Instance.dashDuration + _dashDurationValue);
        if (GameManager.Instance.currencyAmount < _dashDurationPrice)
            _dashDurationButton.interactable = false;

        // dynamite duration upgrade
        _dynamiteDurationText.text = "Dynamite Duration\n¢" + _dynamiteDurationPrice + "\n" + GameManager.Instance.dynamiteDuration + " > "
            + (GameManager.Instance.dynamiteDuration + _dynamiteDurationValue);
        if (GameManager.Instance.currencyAmount < _dynamiteDurationPrice)
            _dynamiteDurationButton.interactable = false;

        // gun duration upgrade
        _gunDurationText.text = "Gun Duration\n¢" + _gunDurationPrice + "\n" + GameManager.Instance.gunDuration + " > "
            + (GameManager.Instance.gunDuration + _gunDurationValue);
        if (GameManager.Instance.currencyAmount < _gunDurationPrice)
            _gunDurationButton.interactable = false;

        /// extra life
        if (!GameManager.Instance.purchasedExtraLife)
        {
            _extraLifeText.text = "Extra Life\n¢" + _extraLifePrice;
            if (GameManager.Instance.currencyAmount < _extraLifePrice)
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
            if (GameManager.Instance.currencyAmount < _headStartPrice)
                _headStartButton.interactable = false;
        }
        // if player has purchased head start, turn off button and change text to purchased
        else
        {
            _headStartButton.interactable = false;
            _headStartText.text = "Head Start\n[Purchased]";
        }
    }

    #region Button Press Functions
    /// Applies corresponding upgrade and updates UI
    public void UpgradeCoinValue()
    {
        GameManager.Instance.coinValue += _coinValue;
        GameManager.Instance.currencyAmount -= _coinPrice;
        SetButtons();
    }
    public void UpgradePlayerClimbSpeed()
    {
        GameManager.Instance.playerMoveSpeed += _playerMoveSpeedValue;
        GameManager.Instance.currencyAmount -= _playerMoveSpeedPrice;
        SetButtons();
    }
    public void UpgradeBoostDuration()
    {
        GameManager.Instance.boostDuration += _boostDurationValue;
        GameManager.Instance.currencyAmount -= _boostDurationPrice;
        SetButtons();
    }
    public void UpgradeDashDuration()
    {
        GameManager.Instance.dashDuration += _dashDurationValue;
        GameManager.Instance.currencyAmount -= _dashDurationPrice;
        SetButtons();
    }
    public void UpgradeDynamiteDuration()
    {
        GameManager.Instance.dynamiteDuration += _dynamiteDurationValue;
        GameManager.Instance.currencyAmount -= _dynamiteDurationPrice;
        SetButtons();
    }
    public void UpgradeGunDuration()
    {
        GameManager.Instance.gunDuration += _gunDurationValue;
        GameManager.Instance.currencyAmount -= _gunDurationPrice;
        SetButtons();
    }
    public void UpgradeExtraLife()
    {
        GameManager.Instance.purchasedExtraLife = true;
        GameManager.Instance.currencyAmount -= _extraLifePrice;
        SetButtons();
    }
    public void UpgradeHeadStart()
    {
        GameManager.Instance.purchasedHeadStart = true;
        GameManager.Instance.currencyAmount -= _headStartPrice;
        SetButtons();
    }
    #endregion
}