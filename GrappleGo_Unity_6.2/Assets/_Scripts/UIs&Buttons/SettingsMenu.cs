using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Devin G Monaghan
/// 11/14/2025
/// Handles settings menu behaviors
///     handles volume settings
///     resets turorials /////////////////////////////// TO BE IMPLEMENTED
/// </summary>

public class SettingsMenu : MonoBehaviour
{
    // reference to volume slider
    [SerializeField] private Slider _volumeSlider;
    // reference to master mixer group
    [SerializeField] private AudioMixer _mixer;

    // settings menu group
    private GameObject _settings_grp;

    #region OnEnable & OnDisable

    private void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.OpenSettings, OnOpenSettings);

        // get reference
        _settings_grp = transform.Find("Settings_grp").gameObject;

        // wait a frame to make sure gamemanager has loaded and set volume
        StartCoroutine(SetVolumeStart());
    }

    private void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.OpenSettings, OnOpenSettings);
    }

    #endregion

    #region Events

    // called when player opens the settings menu
    // called by event
    private void OnOpenSettings()
    {
        // turn on settings
        _settings_grp.SetActive(true);
        // set slider to correct position
        _volumeSlider.value = GameManager.Instance.volume;
    }

    // close the settings menu
    // called by button
    public void CloseSettings()
    {
        // turn off settings
        _settings_grp.SetActive(false);
        EventBus.Publish(EventType.CloseSettings);
    }

    #endregion

    public void ChangeVolume(float value)
    {
        // set volume according to logarithmic formula
        _mixer.SetFloat("Volume", Mathf.Log10(value) * 20);

        // update GameManager's volume
        GameManager.Instance.volume = value;
    }

    // wait a frame and set volume to gamemanager's volume
    private IEnumerator SetVolumeStart()
    {
        yield return null;
        // set volume
        _mixer.SetFloat("Volume", Mathf.Log10(GameManager.Instance.volume) * 20);
    }
}