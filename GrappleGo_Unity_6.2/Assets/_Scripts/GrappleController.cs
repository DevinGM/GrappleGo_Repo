using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 10/17/2025
/// Handles grapple behaviour
/// </summary>

public class GrappleController : MonoBehaviour
{
    // speed grapple falls down
    [SerializeField] private float _fallSpeed = 5f;

    // speed the grapple moves up and down
    private float _grappleSpeed = 12f;
    // spawn position
    private Vector3 _spawnPos;
    // is the grapple currently on the ceiling?
    public bool onCeiling = false;
    // is the grapple currently on the floor?
    private bool _onFloor = false;
    // is the grapple currently on the player?
    private bool _onPlayer = false;

    private Vector3 boxCastSize = new Vector3(.3f, .3f, .3f);

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, EndRun);

        // set spawnPos
        _spawnPos = transform.position;
        // get grapple speed
        _grappleSpeed = GameManager.Instance.grappleClimbSpeed;
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
        onCeiling = false;
        _onPlayer = true;
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
            if (PlayerController.Instance.inputtingGrapple && !onCeiling)
            {
                if (_onFloor)
                    _onFloor = false;
                transform.Translate(_grappleSpeed * Time.deltaTime * transform.up);
            }

            // move down when not inputting grapple but don't let it go farther than the player
            if (!PlayerController.Instance.inputtingGrapple && !_onFloor && !_onPlayer)
            {
                if (onCeiling)
                    onCeiling = false;
                transform.Translate(_fallSpeed * Time.deltaTime * -transform.up);
            }

            // check if grapple has reached floor
            if (transform.position.y <= _spawnPos.y)
                _onFloor = true;

            // check if grapple has reached ceiling
            // detect an object above the grapple
            if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit1, .275f))
            {
                // check if object is the ceiling or a platform and grapple isn't already on the ceiling
                if (hit1.collider.gameObject.CompareTag("Ceiling") && !onCeiling)
                {
                    onCeiling = true;
                    EventBus.Publish(EventType.GrappleHitCeiling);
                }
            }
            else
                onCeiling = false;

            // check if grapple has reached the player
            // detect an object to the left of the grapple
            if (Physics.Raycast(transform.position, -transform.right, out RaycastHit hit2, 1.5f))
            {
                // check if object is the player
                if (hit2.collider.gameObject.CompareTag("Player"))
                    _onPlayer = true;
            }
            else
                _onPlayer = false;

            // raycast to the right of the grapple
            if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit3, 1f))
            {
                // if cast hits a ceiling
                if (hit3.collider.gameObject.CompareTag("Ceiling"))
                {
                    print("grapple ran into platform");
                }
            }
        }
    }
}