using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/30/2025
/// Handles grapple behaviour
/// </summary>

public class GrappleController : MonoBehaviour
{
    // speed the grapple moves up and down
    [SerializeField] private float _grappleSpeed = 5f;

    // spawn position
    private Vector3 _spawnPos;
    // is the grapple currently on the ceiling?
    private bool _onCeiling = false;
    // is the grapple currently on the floor?
    private bool _onFloor = true;
    // reference to grapple rigidbody
    private Rigidbody _rbRef;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, EndRun);

        // set references
        _rbRef = GetComponent<Rigidbody>();

        // set spawnPos
        _spawnPos = transform.position;
    }

    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called when run ends
    private void EndRun()
    {
        _onFloor = true;
        _onCeiling = false;
        transform.position = _spawnPos;
    }

    // Update is called once per frame
    private void Update()
    {
        // only do logic if in run
        if (GameManager.Instance.InRun)
        {
            // while player is in run, move grapple at same speed as player
            transform.Translate(PlayerController.Instance.currentMoveSpeed * Time.deltaTime * transform.right);

            // move up when inputting grapple and not on ceiling
            if (GameManager.Instance.InputtingGrapple && !_onCeiling)
            {
                if (_onFloor)
                    _onFloor = false;
                transform.Translate(_grappleSpeed * Time.deltaTime * transform.up);
            }

            // move down when not inputting grapple and not on floor
            if (!GameManager.Instance.InputtingGrapple && !_onFloor)
            {
                if (_onCeiling)
                    _onCeiling = false;
                transform.Translate(_grappleSpeed * Time.deltaTime * -transform.up);
            }

            // check if grapple has reached floor
            if (transform.position.y <= _spawnPos.y)
                _onFloor = true;

            // check if grapple has reached ceiling
            // detect an object above the player
            if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, .275f))
            {
                // check if object is the ceiling and player isn't already on the ceiling
                if (hit.collider.gameObject.CompareTag("Ceiling") && !_onCeiling)
                {
                    print("Grapple collided with ceiling");
                    _onCeiling = true;
                    EventBus.Publish(EventType.GrappleHitCeiling);
                }
            }
        }
    }
}