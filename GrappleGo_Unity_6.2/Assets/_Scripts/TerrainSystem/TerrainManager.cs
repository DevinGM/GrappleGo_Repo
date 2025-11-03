using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Devin G Monaghan
/// 9/25/2025
/// Handles random terrain generation
/// </summary>

public class TerrainManager : MonoBehaviour
{
    // list of generatable obstacle prefabs.
    // every prefab MUST be 10 meters long
    [Header("List of Chunk Prefabs \nEach chunk MUST be 10 meters long")]
    [SerializeField] private List<GameObject> _chunkList;
    // prefab for beginning 4 chunks of level
    [Header("Prefab of beginning 4 chunks of level")]
    [SerializeField] private GameObject _startingTerrain;
    // last spawned chunk
    private GameObject _lastChunk;

    // reference to terrain mover
    private Transform _terrainMover;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.SpawnChunk, OnSpawnChunk);
        EventBus.Subscribe(EventType.RunEnd, EndRun);

        // get _terrainMover
        _terrainMover = transform.Find("TerrainMover");
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.SpawnChunk, OnSpawnChunk);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called upon end of run
    private void EndRun()
    {
        // spawn starter terrain
        Instantiate(_startingTerrain, Vector3.zero, transform.rotation);
    }

    // spawn a random chunk
    public void OnSpawnChunk()
    {
        int random = Random.Range(0, _chunkList.Count);
        GameObject chunk = Instantiate(_chunkList[random], new Vector3(30f, 0f, 0f), transform.rotation);
        chunk.transform.SetParent(_terrainMover);
        if (_lastChunk != null)
            chunk.transform.position = new Vector3(_lastChunk.transform.position.x + 10f, 0f, 0f);
        _lastChunk = chunk;
    }
}