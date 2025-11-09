using System.Collections;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/7/2024
/// handles gun powerup behavior
/// </summary>

public class GunPowerup : PowerupParent
{
    // reference to bullet prefab
    [SerializeField] private GameObject _bullet;
    // powerup duration
    [SerializeField] private float _duration = 7.5f;
    // how many seconds between shots
    [SerializeField] private float _shootRate = .5f;

    // is the shoot currently on cooldown?
    private bool _shootCooldown = false;
    // reference to gun model
    private GameObject _gunModel;

    protected override void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, OnRunEnd);

        // get reference to in dash model
        _gunModel = transform.Find("GunModel").gameObject;

        // activate upon enabling
        Activate();
    }

    protected override void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunEnd, OnRunEnd);

        Deactivate();
    }

    // called when run ends
    protected override void OnRunEnd()
    {
        this.enabled = false;
    }

    // called upon powerup being activated
    protected override void Activate()
    {
        // set duration
        base.Duration = _duration + GameManager.Instance.dashDuration;

        // tell player they have gun
        PlayerController_Tap.Instance.usingGun = true;

        // activate gun model
        _gunModel.SetActive(true);

        // begin disable timer
        StartCoroutine(AutoDisable());
    }

    // called upon powerup being disactivated
    protected override void Deactivate()
    {
        // tell player they don't have gun
        PlayerController_Tap.Instance.usingGun = false;

        // deactivate gun model
        _gunModel.SetActive(false);
    }

    private void Update()
    {
        // continuosly shoot bullets
        if (!_shootCooldown)
            StartCoroutine(Shoot());
    }

    // shoot bullet
    private IEnumerator Shoot()
    {
        // turn cooldown on
        _shootCooldown = true;

        // spawn a bullet 1 unit in front of the player
        Vector3 spawnPos = transform.position;
        spawnPos.x += 1f;
        Instantiate(_bullet, spawnPos, transform.rotation);

        // wait _shootrate seconds
        yield return new WaitForSeconds(_shootRate);

        // turn cooldown off
        _shootCooldown = false;
    }
}