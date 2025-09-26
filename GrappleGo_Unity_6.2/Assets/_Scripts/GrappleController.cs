using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/26/2025
/// Handles grapple behaviour
/// </summary>

public class GrappleController : MonoBehaviour
{
    // speed the grapple moves up and down
    [SerializeField] private float _grappleSpeed = 5f;

    // spawn y height
    private Vector3 _spawnPos;
    // height of when the player last hit the ceiling
    private float _ceilingHeight = 0f;
    // is the grapple currently on the ceiling?
    private bool _onCeiling = false;
    // is the grapple currently on the player?
    private bool _onPlayer = true;
    // is the player currently in a run?
    private bool _inRun = false;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);

        // set spawnPos
        _spawnPos = transform.localPosition;
    }

    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called when run starts
    private void StartRun()
    {
        _inRun = true;
    }

    // called when run ends
    private void EndRun()
    {
        _inRun = false;
        _onPlayer = true;
        _onCeiling = false;
        transform.localPosition = _spawnPos;
    }

    // Update is called once per frame
    private void Update()
    {
        // only do logic if in run
        if (_inRun)
        {
            // move up when inputting grapple and not on ceiling
            if (PlayerController.Instance.InputtingGrapple && !_onCeiling)
                transform.Translate(_grappleSpeed * Time.deltaTime * transform.up);

            // move down when not inputting grapple and not on floor
            if (!PlayerController.Instance.InputtingGrapple && !_onPlayer)
                transform.Translate(_grappleSpeed * Time.deltaTime * -transform.up);

            // detect when on the player
            if (transform.localPosition.y < _spawnPos.y)
            {
                // tell player if they reached the grapple while on the ceiling
                if (_onCeiling)
                    EventBus.Publish(EventType.PlayerHitCeiling);
                // only reset grapple position when not on the ceiling
                else
                    transform.localPosition = _spawnPos;

                _onPlayer = true;
            }
            else
                _onPlayer = false;
        }
    }

    // handles trigger collision enters
    public void OnTriggerEnter(Collider other)
    {
        // detect when on ceiling
        if (other.CompareTag("Ceiling"))
        {
            _onCeiling = true;
            _ceilingHeight = transform.position.y;
            EventBus.Publish(EventType.GrappleHitCeiling);
        }
    }

    // handles trigger collision exits
    public void OnTriggerExit(Collider other)
    {
        // detect when off ceiling
        if (other.CompareTag("Ceiling"))
        {
            // only announce player has left ceiling when grapple moves beneath ceiling height
            if (_ceilingHeight > transform.position.y)
                _onCeiling = false;
            // if grapple moved up, reset to ceiling height
            else if (_ceilingHeight < transform.position.y)
                transform.position = new Vector3(transform.position.x, _ceilingHeight, 0f);
        }
    }
}