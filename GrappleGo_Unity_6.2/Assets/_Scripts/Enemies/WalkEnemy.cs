using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 10/19/2025
/// Handles walk enemy behaviour
/// </summary>

public class WalkEnemy : MonoBehaviour, IEnemy
{
    // movement speed
    [SerializeField] private float _moveSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        // only do logic if in run
        if (GameManager.Instance.InRun)
        {
            Movement();
        }
    }

    // movement behaviour
    public void Movement()
    {
        transform.Translate(_moveSpeed * Time.deltaTime * -transform.right);
    }
}