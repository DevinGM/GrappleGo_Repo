using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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
    private bool _inRun = false;
    public bool InRun { get { return _inRun; } }

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called when run begins
    public void StartRun()
    {
        _inRun = true;
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
        {
            Movement();
        }
    }

    // movement behaviour
    public void Movement()
    {
        transform.position += _moveSpeed * Time.deltaTime * -transform.right;
    }
}