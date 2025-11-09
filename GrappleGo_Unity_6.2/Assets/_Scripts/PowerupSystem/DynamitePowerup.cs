using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Devin G Monaghan
/// 11/7/2024
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
    private PlayerInputs _playerInputs;
    private InputAction _dynamiteAction;

    protected override void OnEnable()
    {
        // add inputs
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();
        _dynamiteAction = _playerInputs.Controls.Dynamite;
        _dynamiteAction.performed += Use;

        // get explosion reference
        _explosionRef = transform.Find("Explosion").gameObject;
    }

    protected override void OnDisable()
    {
        // unsubscribe to inputs
        _dynamiteAction.performed -= Use;
    }

    // called upon powerup being activated
    public override void Pickup()
    {
        // publish GetDynamite
        EventBus.Publish(EventType.GetDynamite);
    }

    // trigger dash on button press
    protected override void Use(InputAction.CallbackContext context)
    {
        if (!_cooldown && PlayerController_Tap.Instance.DynamiteCharges > 0)
        {
            _cooldown = true;
            StartCoroutine(Dynamite());
        }
    }

    // perform dash
    private IEnumerator Dynamite()
    {
        // spawn explosion by turning it on
        _explosionRef.SetActive(true);

        // publish Dynamite
        EventBus.Publish(EventType.UseDynamite);

        // wait _cooldownLength seconds
        yield return new WaitForSeconds(_cooldownLength);

        // turn off cooldown
        _cooldown = false;
    }
}