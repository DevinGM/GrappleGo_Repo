using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 10/19/2025
/// Handles turret bullet behaviour
/// </summary>

public class TurretBullet : MonoBehaviour
{
    // movement speed
    [SerializeField] private float _moveSpeed = 3f;

    private void OnEnable()
    {
        // start death timer upon spawn
        StartCoroutine(DeathTimer());
    }

    // Update is called once per frame
    void Update()
    {
        // move forward continuously
        transform.position += _moveSpeed * Time.deltaTime * transform.forward;
    }

    // wait 5 seconds before culling self
    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}