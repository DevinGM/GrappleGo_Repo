using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/7/2025
/// Handles player bullet behaviour
/// </summary>

public class PlayerBullet : MonoBehaviour
{
    // movement speed
    private float _moveSpeed = 7f;

    private void OnEnable()
    {
        // start death timer upon spawn
        StartCoroutine(DeathTimer());
    }

    // Update is called once per frame
    private void Update()
    {
        // move right continuously
        transform.position += _moveSpeed * Time.deltaTime * transform.right;
    }

    // wait 3 seconds before culling self
    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    // handles trigger interactions
    private void OnTriggerEnter(Collider other)
    {
        // if bullet collides with enemy, kill enemy
        if (other.gameObject.CompareTag("Enemy"))
            Destroy(other.gameObject);

        // if bullet collides with something other than player or spawn line detecter, destroy bullet
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("SpawnLineDetecter"))
            Destroy(this.gameObject);
    }
}