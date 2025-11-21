using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/13/2025
/// Handles pause menu behaviors
/// </summary>

public class PauseMenu : MonoBehaviour
{
    // refernce to pause group
    private GameObject _pause_grp;

    [Header("Text Boxes")]
    // references to text boxes
    [SerializeField] private TMP_Text _highscoreDisplay;
    [SerializeField] private TMP_Text _scoreDisplay;

    #region OnEnable & OnDisable

    private void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.Pause, OnPause);
        EventBus.Subscribe(EventType.CloseTutorial, Unpause);

        // get reference
        _pause_grp = transform.Find("Pause_grp").gameObject;
    }

    private void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.Pause, OnPause);
        EventBus.Unsubscribe(EventType.CloseTutorial, Unpause);

        // make sure time scale is set to normal
        Time.timeScale = 1.0f;
    }

    #endregion

    // pause game by setting timeScale to 0
    // called by event
    private void OnPause()
    {
        // pause game
        Time.timeScale = 0f;
        // turn pause menu on
        _pause_grp.SetActive(true);
        // set highscore and score text
        _highscoreDisplay.text = "Highscore: " + GameManager.Instance.highScore;
        _scoreDisplay.text = "Score: " + (GameManager.Instance.distanceScore + GameManager.Instance.pickupsScore);
    }

    // unpauses game
    // called by button press
    // called by event via close tutorial
    public void Unpause()
    {
        // publish unpause
        EventBus.Publish(EventType.Unpause);
        // unpause game
        StartCoroutine(UnpauseBuffer());
        // turn pause menu off
        _pause_grp.SetActive(false);
    }

    // give the player 3 seconds to get ready before unpausing
    private IEnumerator UnpauseBuffer()
    {
        // wait 3 seconds for the countdown
        yield return new WaitForSecondsRealtime(3f);
        // set time scale to normal
        Time.timeScale = 1.0f;
    }
}