using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Devin G Monaghan
/// 9/16/2025
/// Handles chunk behaviours
/// </summary>

public class ChunkController : MonoBehaviour
{
    public IObjectPool<ChunkController> Pool { get; set; }

    // is the player currently in a run?
    private bool _inRun = false;
    // has this chunk been stepped on already?
    private bool _steppedOn = false;
    // was this chunk placed by hand or by the terrainManager?
    [Header("Was this chunk placed by hand? \nWill throw error if true but not checked")]
    [SerializeField] private bool _manualPlaced = false;

    // does this chunk have consumables the player could pick up or kill?
    [Header("Does this chunk have consumables? \nCheck if chunk has coins, enemies, etc " +
        "\nso the oject pool doesn't reuse a chunk with missing coins etc")]
    public bool consumable = false;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    private void Update()
    {
        // if the player gets too far away cull this chunk
        if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) > 50f)
            OnReturnToPool();
    }

    // called when run begins
    public void StartRun()
    {
        _inRun = true;
    }
    // called when run ends
    private void EndRun()
    {
        _inRun = false;
    }

    // called when object collides with a collider
    private void OnCollisionEnter(Collision collision)
    {
        // only perform logic during a run
        if (_inRun)
        {
            if (!_steppedOn && collision.gameObject.CompareTag("Player"))
            {
                EventBus.Publish(EventType.ChunkSteppedOn);
                _steppedOn = true;
            }
        }
    }

    // release this chunk into the pool
    private void OnReturnToPool()
    {
        // reset stepped on status
        _steppedOn = false;

        if (!_manualPlaced)
            Pool.Release((ChunkController)this);
        else
            Destroy(this.gameObject);
    }
}