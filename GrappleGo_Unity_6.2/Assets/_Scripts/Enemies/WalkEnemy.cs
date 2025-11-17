using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/5/2025
/// Handles walk enemy behaviour
/// </summary>

public class WalkEnemy : MonoBehaviour, IEnemy
{
    // movement speed
    [SerializeField] private float _moveSpeed = 10f;

    // Update is called once per frame
    public void Update()
    {
        // only do logic if in run
        if (GameManager.Instance.InRun)
            Action();
    }

    // movement behaviour
    public void Action()
    {
        transform.Translate(_moveSpeed * Time.deltaTime * -transform.right);
    }
}