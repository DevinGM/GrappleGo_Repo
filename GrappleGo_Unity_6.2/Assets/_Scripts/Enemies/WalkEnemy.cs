using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/30/2025
/// Handles walk enemy behaviour
/// </summary>

public class WalkEnemy : MonoBehaviour, IEnemy
{
    // movement speed
    [SerializeField] private float _moveSpeed = 10f;

    // is this enemy dead?
    public bool Dead { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        // only do logic if in run
        if (GameManager.Instance.InRun)
        {
            Movement();

            if (Dead)
                Destroy(this.gameObject);
        }
    }

    // movement behaviour
    public void Movement()
    {
        transform.Translate(_moveSpeed * Time.deltaTime * -transform.right);
    }
}