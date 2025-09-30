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
    // x position chunk spawns at, defaults to 40
    [Header("Spawn X Coordinate of Chunk \nSet to coordinate first chunk will spawn at")]
    [SerializeField] private float _startSpawnXPos = 40f;

    // spawn position of chunks, gets updated during run and reset at end
    private float _currentSpawnXPos = 40f;
    
    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.ChunkSteppedOn, SpawnChunk);
        EventBus.Subscribe(EventType.RunEnd, EndRun);

        // set current spawn posiiton
        _currentSpawnXPos = _startSpawnXPos;
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.ChunkSteppedOn, SpawnChunk);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called upon end of run
    private void EndRun()
    {
        // spawn starter terrain
        Instantiate(_startingTerrain, Vector3.zero, transform.rotation);
        // reset current spawn posiiton
        _currentSpawnXPos = _startSpawnXPos;
    }

    // spawn a random chunk and place it in line
    public void SpawnChunk()
    {
        int random = Random.Range(0, _chunkList.Count);
        GameObject chunk = Instantiate(_chunkList[random], new Vector3(_startSpawnXPos, 0f, 0f), transform.rotation);
        ChunkController chunkController = chunk.GetComponent<ChunkController>();
        chunkController.StartRun();
        chunk.transform.position = new Vector3(_currentSpawnXPos, 0f, 0f);
        _currentSpawnXPos += 10f;
    }
}