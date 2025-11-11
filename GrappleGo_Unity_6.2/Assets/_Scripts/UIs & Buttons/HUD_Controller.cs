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

    [Header("List of Dynamite Charges. Place in matching order")]
    // references to dynamite charges
    [SerializeField] private List<UnityEngine.UI.Image> _dynamiteChargesList;

    private void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.DashStart, TurnOnDash);
        EventBus.Subscribe(EventType.DashEnd, TurnOffDash);
        EventBus.Subscribe(EventType.GetDynamite, OnGetDynamite);
        EventBus.Subscribe(EventType.UseDynamite, OnUseDynamite);
        EventBus.Subscribe(EventType.RunEnd, TurnOffButtons);

        // check for 4th and 5th dynamite charges
        if (GameManager.Instance.maxDynamiteCharges >= 4)
            _dynamiteChargesList[3].gameObject.SetActive(true);
        if (GameManager.Instance.maxDynamiteCharges >= 5)
            _dynamiteChargesList[4].gameObject.SetActive(true);

        // empty all charges on level begin
        EmptyCharges(_dynamiteChargesList, _dynamiteButton);
    }

    private void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.DashStart, TurnOnDash);
        EventBus.Unsubscribe(EventType.DashEnd, TurnOffDash);
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

    // turn on dash button
    private void TurnOnDash()
    {
        _dashButton.SetActive(true);
    }
    // turn off dash button
    private void TurnOffDash()
    {
        _dashButton.SetActive(false);
    }

    // turn off buttons
    private void TurnOffButtons()
    {
        _dashButton.SetActive(false);
        _dynamiteButton.SetActive(false);
    }

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

}