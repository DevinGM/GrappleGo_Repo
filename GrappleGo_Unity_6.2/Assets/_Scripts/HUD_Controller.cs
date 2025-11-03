using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/1/2025
/// Handles level hud behaviors
/// </summary>

public class HUD_Controller : MonoBehaviour
{
    private GameObject dashButton;
    private GameObject gunButton;
    private GameObject dynamiteButton;

    private void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.DashStart, OnDashStart);
        EventBus.Subscribe(EventType.DashEnd, TurnOffButtons);
        EventBus.Subscribe(EventType.RunEnd, TurnOffButtons);

        // get buttons
        dashButton = transform.Find("Dash Button").gameObject;
        gunButton = transform.Find("Gun Button").gameObject;
        dynamiteButton = transform.Find("Dynamite Button").gameObject;
    }

    private void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.DashStart, OnDashStart);
        EventBus.Unsubscribe(EventType.DashEnd, TurnOffButtons);
        EventBus.Unsubscribe(EventType.RunEnd, TurnOffButtons);
    }

    // turn on dash button
    private void OnDashStart()
    {
        dashButton.SetActive(true);
    }

    // turn off all buttons
    private void TurnOffButtons()
    {
        dashButton.SetActive(false);
        gunButton.SetActive(false);
        dynamiteButton.SetActive(false);
    }
}