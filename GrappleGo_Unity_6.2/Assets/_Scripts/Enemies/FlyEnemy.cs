using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/25/2025
/// Handles flying enemy behaviour
/// </summary>

public class FlyEnemy : MonoBehaviour, IEnemy
{
    // movement speed
    [SerializeField] private float _moveSpeed = 5f;
    // is the enemy moving to point A or point B?
    [SerializeField] private bool _movingAtoB = true;

    // references to children
    private Transform _model;
    private Transform _pointA;
    private Transform _pointB;

    // is the player currently in a run?
    private bool _inRun = true;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, EndRun);

        // get references to children
        _model = this.transform.Find("Model");
        _pointA = this.transform.Find("PointA");
        _pointB = this.transform.Find("PointB");
        if (_model == null || _pointA == null || _pointB == null)
            Debug.LogError("ERROR: Flying enemy could not find at least one of its children!!!!");
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
        // move towards point B
        if (_movingAtoB)
        {
            _model.position = Vector3.MoveTowards(_model.position, _pointB.position, _moveSpeed * Time.deltaTime);
            if (_model.position == _pointB.position)
                _movingAtoB = !_movingAtoB;

        }
        // move towards point A
        else
        {
            _model.position = Vector3.MoveTowards(_model.position, _pointA.position, _moveSpeed * Time.deltaTime);
            if (_model.position == _pointA.position)
                _movingAtoB = !_movingAtoB;

        }
    }
}