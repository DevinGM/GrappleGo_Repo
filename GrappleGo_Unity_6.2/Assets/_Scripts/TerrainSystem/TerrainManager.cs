using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/16/2025
/// Handles random terrain generation
/// </summary>

public class TerrainManager : MonoBehaviour
{
    // list of generatable obstacle prefabs.
    // every prefab MUST be 10 meters long
    [Header("List of Chunk Prefabs \nEach chunk MUST be 10 meters long")]
    public List <GameObject> chunkList;

    // x position chunk spawns at, defaults to 30
    [Header("Spawn X Coordinate of Chunk \nSet to coordinate first chunk will spawn at")]
    [SerializeField] private float _spawnXPos = 30f;
    
    // is the player currently in a run?
    private bool _inRun = false;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);
        EventBus.Subscribe(EventType.ChunkSteppedOn, SpawnChunk);
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
        EventBus.Unsubscribe(EventType.ChunkSteppedOn, SpawnChunk);
    }

    // randomly select a chunk and spawn it in line
    private void SpawnChunk()
    {
        // only do logic if in run
        if (_inRun)
        {
            // make sure there are chunks in the list
            if (chunkList.Count > 0)
            {
                int random = Random.Range(0, chunkList.Count);
                GameObject spawnedChunk = Instantiate(chunkList[random], new Vector3(_spawnXPos, 0f, 0f), transform.rotation);
                spawnedChunk.GetComponent<ChunkController>().StartRun();
            }
            // there are no chunks so return an error
            else
            {
                Debug.LogError("ERROR: No chunks given to terrain manager");
            }

            _spawnXPos += 10f;
        }
    }

    // called when run begins
    private void StartRun()
    {
        _inRun = true;
    }
    // called when run ends
    private void EndRun()
    {
        _inRun = false;
    }
}