using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/14/2025
/// Handles triggering settings to open
/// Handles hiding given ui when opening settings
/// </summary>

public class OpenSettings : MonoBehaviour
{
    // reference to ui group hidden when settings open
    [SerializeField] private GameObject _ui_grp;

    private void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.CloseSettings, OnCloseSettings);
    }

    private void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.CloseSettings, OnCloseSettings);
    }

    #region Open/Close Settings

    // turns off menu and tells settings menu to turn on
    // called by button press
    public void OpenSettingsButton()
    {
        // turn menu off
        _ui_grp.SetActive(false);
        // publish open settings
        EventBus.Publish(EventType.OpenSettings);
    }

    // turn menu back on
    // called by event
    private void OnCloseSettings()
    {
        // turn menu on
        _ui_grp.SetActive(true);
    }

    #endregion
}