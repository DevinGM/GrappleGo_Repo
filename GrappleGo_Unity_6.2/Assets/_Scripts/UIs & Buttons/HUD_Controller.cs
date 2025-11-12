using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Devin G Monaghan
/// 11/3/2025
/// Handles level hud behaviors
/// </summary>

public class HUD_Controller : MonoBehaviour
{
    [Header("Buttons")]
    // references to powerup buttons
    [SerializeField] private GameObject _dashButton;
    [SerializeField] private GameObject _dynamiteButton;

    [Header("Text Boxes")]
    // references to text boxes
    [SerializeField] private TMP_Text _highscoreDisplay;
    [SerializeField] private TMP_Text _scoreDisplay;

    [Header("List of Dash Charges. Place in matching order")]
    // references to dynamite charges
    [SerializeField] private List<UnityEngine.UI.Image> _dashChargesList;
    [Header("List of Dynamite Charges. Place in matching order")]
    // references to dynamite charges
    [SerializeField] private List<UnityEngine.UI.Image> _dynamiteChargesList;

    private void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.GetDash, OnGetDash);
        EventBus.Subscribe(EventType.UseDash, OnUseDash);
        EventBus.Subscribe(EventType.GetDynamite, OnGetDynamite);
        EventBus.Subscribe(EventType.UseDynamite, OnUseDynamite);
        EventBus.Subscribe(EventType.RunEnd, TurnOffButtons);

        // check for 4th and 5th dash charges
        if (GameManager.Instance.maxDynamiteCharges >= 4)
            _dashChargesList[3].gameObject.SetActive(true);
        if (GameManager.Instance.maxDynamiteCharges >= 5)
            _dashChargesList[4].gameObject.SetActive(true);
        // check for 4th and 5th dynamite charges
        if (GameManager.Instance.maxDynamiteCharges >= 4)
            _dynamiteChargesList[3].gameObject.SetActive(true);
        if (GameManager.Instance.maxDynamiteCharges >= 5)
            _dynamiteChargesList[4].gameObject.SetActive(true);

        // empty all charges on level begin
        EmptyCharges(_dynamiteChargesList, _dynamiteButton);
        EmptyCharges(_dashChargesList, _dashButton);
    }

    private void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.GetDash, OnGetDash);
        EventBus.Unsubscribe(EventType.UseDash, OnUseDash);
        EventBus.Unsubscribe(EventType.GetDynamite, OnGetDynamite);
        EventBus.Unsubscribe(EventType.UseDynamite, OnUseDynamite);
        EventBus.Unsubscribe(EventType.RunEnd, TurnOffButtons);
    }

    // update is called every frame
    private void Update()
    {
        // // set highscore and score text
        _highscoreDisplay.text = "Highscore: " + GameManager.Instance.highScore;
        _scoreDisplay.text = "Score: " + (GameManager.Instance.distanceScore + 
            GameManager.Instance.pickupsScore);
    }

    // turn off buttons
    private void TurnOffButtons()
    {
        _dashButton.SetActive(false);
        _dynamiteButton.SetActive(false);
    }

    #region Empty & Fill Charges

    // empty all of the given list's charges and turn off given button
    private void EmptyCharges(List<UnityEngine.UI.Image> chargesList, GameObject button)
    {
        // empty all charges
        for (int i = 0; i < chargesList.Count; i++)
            chargesList[i].color = new Color(.3f, .3f, .3f, .75f);
            
        // turn off button
        button.SetActive(false);
    }

    // fill all of the given list's charges until the given current amount of charges
    // if any charges were filled and the given button is off, turn on the button
    private void FillCharges(List<UnityEngine.UI.Image> chargesList, int currentCharges, GameObject button)
    {
        bool filledACharge = false;
        // refill charges up to player's current number of charges
        for (int i = 0; i < currentCharges; i++)
        {
            chargesList[i].color = new Color(1f, 1f, 1f, .75f);
            filledACharge = true;

            print("Filled charge at: " + chargesList[i]);
        }

        // if any charges were filled and the button is off, turn on button
        if (filledACharge && !button.activeSelf)
            button.SetActive(true);
    }

    #endregion

    #region Dash

    // called when player picks up a dash
    private void OnGetDash()
    {
        StartCoroutine(GetDashBuffer());
    }

    // wait for one frame to make sure player has updated charges count and fill dash charges
    private IEnumerator GetDashBuffer()
    {
        yield return null;
        FillCharges(_dashChargesList, PlayerController_Tap.Instance.DashCharges, _dashButton);
    }

    // called when player uses a dash
    private void OnUseDash()
    {
        StartCoroutine(UseDashBuffer());
    }

    // wait for one frame to make sure player has updated charges count and empty and fill dash charges
    private IEnumerator UseDashBuffer()
    {
        yield return null;
        EmptyCharges(_dashChargesList, _dashButton);
        FillCharges(_dashChargesList, PlayerController_Tap.Instance.DashCharges, _dashButton);
    }

    #endregion
    
    #region Dynamite

    // called when player picks up a dynamite
    private void OnGetDynamite()
    {
        StartCoroutine(GetDynamiteBuffer());
    }

    // wait for one frame to make sure player has updated charges count and fill dynamite charges
    private IEnumerator GetDynamiteBuffer()
    {
        yield return null;
        FillCharges(_dynamiteChargesList, PlayerController_Tap.Instance.DynamiteCharges, _dynamiteButton);
    }

    // called when player uses a dynamite
    private void OnUseDynamite()
    {
        StartCoroutine(UseDynamiteBuffer());
    }

    // wait for one frame to make sure player has updated charges count and empty and fill dynamite charges
    private IEnumerator UseDynamiteBuffer()
    {
        yield return null;
        EmptyCharges(_dynamiteChargesList, _dynamiteButton);
        FillCharges(_dynamiteChargesList, PlayerController_Tap.Instance.DynamiteCharges, _dynamiteButton);
    }

    #endregion

}