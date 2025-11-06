using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/5/2025
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

    public void OnEnable()
    {
        // get references to children
        _model = this.transform.Find("Model");
        _pointA = this.transform.Find("PointA");
        _pointB = this.transform.Find("PointB");
        if (_model == null || _pointA == null || _pointB == null)
            Debug.LogError("ERROR: Flying enemy could not find at least one of its children!!!!");
    }

    // Update is called once per frame
    public void Update()
    {
        // only do logic if in run
        if (GameManager.Instance.InRun)
        {
            Action();
        }
    }

    // movement behaviour
    public void Action()
    {
        // move towards point B
        if (_movingAtoB)
        {
            // make sure enemy isn't dead
            if (_model != null)
            {
                _model.position = Vector3.MoveTowards(_model.position, _pointB.position, _moveSpeed * Time.deltaTime);
                if (_model.position == _pointB.position)
                    _movingAtoB = !_movingAtoB;
            }
        }
        // move towards point A
        else
        {
            // make sure enemy isn't dead
            if (_model != null)
            {
                _model.position = Vector3.MoveTowards(_model.position, _pointA.position, _moveSpeed * Time.deltaTime);
                if (_model.position == _pointA.position)
                    _movingAtoB = !_movingAtoB;
            }
        }
    }
}