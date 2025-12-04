using NUnit.Framework.Interfaces;
using System.Collections;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/20/2025
/// Handles grapple moving back towards player when idle
/// </summary>

public class Grapple : MonoBehaviour
{
    // number of seconds player must idle before grapple moves
    [SerializeField] private float _idleLength = 1f;
    // speed grapple moves at
    [SerializeField] private float _speed = 7f;

    private bool _idle = false;

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.PlayerStartMove, OnPlayerStartMove);
        EventBus.Subscribe(EventType.PlayerStopMove, OnPlayerStopMove);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.PlayerStartMove, OnPlayerStartMove);
        EventBus.Unsubscribe(EventType.PlayerStopMove, OnPlayerStopMove);
    }

    // stop idle timer & stop moving
    // called by event
    private void OnPlayerStartMove()
    {
        StopCoroutine(IdleTimer());
        _idle = false;
    }

    // start idle timer
    // called by event
    private void OnPlayerStopMove()
    {
        StartCoroutine(IdleTimer());
    }

    // wait _idleLength seconds before allowing grapple to move
    private IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(_idleLength);
        _idle = true;
    }

    private void Update()
    {
        // only do logic in run
        if (GameManager.Instance.InRun)
        {
            // make sure not to move grapple when player is moving
            if (PlayerController_Tap.Instance.moving)
                _idle = false;

            // if player is idle move grapple towards them
            if (_idle)
            {
                // get position of player and align it to grapple's z
                Vector3 playerPos = PlayerController_Tap.Instance.transform.position;
                playerPos.z = -2;

                // if grapple is farther than 1.25 units away from the player, move towards the player
                if (Vector3.Distance(playerPos, transform.position) > 1.25f)
                    transform.position = Vector3.MoveTowards(transform.position, playerPos, _speed * Time.deltaTime);
            }
        }
    }
}