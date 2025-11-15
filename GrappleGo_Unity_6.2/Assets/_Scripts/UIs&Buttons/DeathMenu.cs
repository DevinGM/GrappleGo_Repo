using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Devin G Monaghan
/// 11/3/2025
/// Handles death menu textboxes
/// </summary>

public class DeathMenu : MonoBehaviour
{
    // reference to text objects
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _currencyText;

    private void OnEnable()
    {
        if (_scoreText != null)
            _scoreText.text = "Score: " + GameManager.Instance.lastScore;
        if (_currencyText != null)
            _currencyText.text = "Total Coins: " + GameManager.Instance.currencyAmount;
    }
}