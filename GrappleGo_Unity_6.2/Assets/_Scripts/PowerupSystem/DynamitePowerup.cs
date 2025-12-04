using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Devin G Monaghan
/// 11/11/2024
/// handles dynamite powerup behavior
/// </summary>

public class DynamitePowerup : PowerupChargeParent
{
    // is the dynamite currently on cooldown?
    private bool _cooldown = false;
    private float _cooldownLength = 1f;
    // reference to explosion
    private GameObject _explosionRef;
    // references to inputs
    private InputAction _dynamiteAction;

    protected override void OnEnable()
    {
        // add inputs
        StartCoroutine(AddInputs());

        // get explosion reference
        _explosionRef = transform.Find("Explosion").gameObject;
    }

    // wait one frame to add inputs to make sure player has correct reference
    private IEnumerator AddInputs()
    {
        yield return null;
        _dynamiteAction = PlayerController_Tap.Instance.PlayerInputs.Controls.Dynamite;
        _dynamiteAction.performed += Use;
    }

    protected override void OnDisable()
    {
        // disable inputs
        _dynamiteAction.performed -= Use;
    }

    // called upon powerup being picked up
    public override void Pickup()
    {
        // if dynamite tutorial has not been played, play it
        if (!GameManager.Instance.playedDynamiteTutorial)
        {
            GameManager.Instance.playedDynamiteTutorial = true;
            EventBus.Publish(EventType.PlayDynamiteTutorial);
        }

        // publish GetDynamite
        EventBus.Publish(EventType.GetDynamite);
    }

    // trigger dynamite on button press
    protected override void Use(InputAction.CallbackContext context)
    {
        if (!_cooldown && PlayerController_Tap.Instance.DynamiteCharges > 0)
            StartCoroutine(Dynamite());
    }

    // perform dynamite
    private IEnumerator Dynamite()
    {
        // turn on cooldown
        _cooldown = true;

        // spawn explosion by turning it on
        _explosionRef.SetActive(true);

        // publish UseDynamite
        EventBus.Publish(EventType.UseDynamite);

        // wait _cooldownLength seconds
        yield return new WaitForSeconds(_cooldownLength);

        // turn off cooldown
        _cooldown = false;
    }
}