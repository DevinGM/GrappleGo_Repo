using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 12/2/2025
/// Handles turret enemy behaviour
/// </summary>

public class TurretEnemy : MonoBehaviour, IEnemy
{
    // length of shooting cooldown
    [SerializeField] private float _shootRate = 3f;
    // is the turret curently on shooting cooldown?
    [SerializeField] private bool _onShootCooldown = false;
    // has the player passed the turrent, and as such should the turret stop firing?
    [SerializeField] private bool _playerPassed = false;
    // reference to bullet prefab
    [SerializeField] private GameObject _bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        // only do logic if in run
        if (GameManager.Instance.InRun)
        {
            // shoot continuously while not on cooldown and not passed by player
            if (!_onShootCooldown && !_playerPassed)
                Action();

            // if player passes the turret, stop firing
            if (PlayerController_Tap.Instance.transform.position.x >= transform.position.x)
                _playerPassed = true;
        }
    }

    // shoot a bullet towards 5 units in front of the player
    public void Action()
    {
        // set bullet's spawn location to one unit to the left and one unit above the turret
        /// /////////////////////////////////////// UPDATE WITH MODEL DIMENSIONS
        Vector3 spawnPos = transform.position;
        spawnPos.x--;
        spawnPos.y++;
        // spawn bullet
        GameObject bullet = Instantiate(_bulletPrefab, spawnPos, transform.rotation);
        // start shoot cooldown
        StartCoroutine(ShootCooldown());

        // play shoot sound
        if (EnemyAudioHandler.Instance.enemyShoot != null)
            EnemyAudioHandler.Instance.PlaySound(EnemyAudioHandler.Instance.enemyShoot);
    }

    // turn on cooldown, wait _shootRate seconds, turn cooldown off
    public IEnumerator ShootCooldown()
    {
        _onShootCooldown = true;
        yield return new WaitForSeconds(_shootRate);
        _onShootCooldown = false;
    }
}