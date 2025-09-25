using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Devin G Monaghan
/// 9/24/2025
/// Handles chunk behaviours
/// </summary>

public class ChunkController : MonoBehaviour
{
    // reference to player detecter
    private PlayerDetecter _detecterRef;
    // is the player currently in a run?
    private bool _inRun = false;

    // was this chunk placed by hand or by the terrainManager?
    [Header("Was this chunk placed by hand? \nWill throw error if true but not checked")]
    [SerializeField] private bool _manualPlaced = false;

    // does this chunk have consumables the player could pick up or kill?
    [Header("Does this chunk have consumables? \nCheck if chunk has coins, enemies, etc " +
        "\nso the oject pool doesn't reuse a chunk with missing coins etc")]
    public bool consumable = false;
    // reference to object pool
    public IObjectPool<ChunkController> Pool { get; set; }

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);

        // get decter ref
        _detecterRef = transform.Find("PlayerDetecter").gameObject.GetComponent<PlayerDetecter>();
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    private void Update()
    {
        if (_inRun)
        {
            // if the player gets too far away cull this chunk
            if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) > 50f)
                OnReturnToPool();
        }
    }

    // called when run begins
    public void StartRun()
    {
        _inRun = true;
        _detecterRef.StartRun();
    }
    // called when run ends
    private void EndRun()
    {
        Destroy(this.gameObject);
    }

    // release this chunk into the pool
    private void OnReturnToPool()
    {
        // reset stepped on status
        _detecterRef.steppedOn = false;

        if (!_manualPlaced)
            Pool.Release((ChunkController)this);
        else
            Destroy(this.gameObject);
    }
}