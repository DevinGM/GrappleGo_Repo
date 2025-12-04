using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 12/2/2025
/// Handles turret bullet behaviour
/// </summary>

public class TurretBullet : MonoBehaviour
{
    // movement speed
    [SerializeField] private float _moveSpeed = 3f;

    // location bullet moves 
    public Vector3 targetPos;

    private void OnEnable()
    {
        // start death timer upon spawn
        StartCoroutine(DeathTimer());
        // rotate bullet to look at player
        transform.LookAt(PlayerController_Tap.Instance.transform.position);
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