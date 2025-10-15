using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 10/14/2025
/// Turns on starting powerups when player has purchased according upgrades
/// </summary>

public class StartingPowerups : MonoBehaviour
{
    // reference to powerup objects
    [SerializeField] private GameObject shieldPowerup;
    [SerializeField] private GameObject boostPowerup;

    private void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, OnRunStart);
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);
    }
    private void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunStart, OnRunStart);
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);
    }

    // called when run begins
    private void OnRunStart()
    {
        // if player has purchased the extra life upgrade, find and turn on the starting shield powerup
        if (GameManager.Instance.purchasedExtraLife)
            shieldPowerup.gameObject.SetActive(true);
        // if player has purchased the head start upgrade, find and turn on the starting boost powerup
        if (GameManager.Instance.purchasedHeadstart)
            boostPowerup.gameObject.SetActive(true);
    }

    // called when run ends
    private void OnRunEnd()
    {
        Destroy(this.gameObject);
    }
}