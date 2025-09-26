using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/19/2025
/// Handles walk enemy behaviour
/// </summary>

public class WalkEnemy : MonoBehaviour, IEnemy
{
    // movement speed
    [SerializeField] private float _moveSpeed = 10f;

    // is the player currently in a run?
    private bool _inRun = true;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, EndRun);
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called when run ends
    public void EndRun()
    {
        _inRun = false;
    }

    // Update is called once per frame
    void Update()
    {
        // only do logic if in run
        if (_inRun)
            Movement();
    }

    // movement behaviour
    public void Movement()
    {
        transform.position += _moveSpeed * Time.deltaTime * -transform.right;
    }
}