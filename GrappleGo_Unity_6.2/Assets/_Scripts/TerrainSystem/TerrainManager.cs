using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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

    [Header("Max number of reserved chunks")]
    [SerializeField] private int _maxPoolSize = 10;
    [Header("Initial amount of reserved memory reserved for pool")]
    [SerializeField] private int _stackDefaultCapacity = 10;

    // pool of chunks
    private IObjectPool<ChunkController> _pool;

    // property referencing private pool
    public IObjectPool<ChunkController> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<ChunkController>(CreatedPooledItem, OnTakeFromPool,
                    OnReturnedFromPool, OnDestroyPoolObject, true, _stackDefaultCapacity, _maxPoolSize);
            }
            return _pool;
        }
    }

    // is the player currently in a run?
    //private bool _inRun = false;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.ChunkSteppedOn, SpawnChunk);
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.ChunkSteppedOn, SpawnChunk);
    }

    // instantiates a new random chunk
    // returns chunk's controller
    private ChunkController CreatedPooledItem()
    {
        // make sure there are chunks in the list
        if (chunkList.Count > 0)
        {
            int random = Random.Range(0, chunkList.Count);
            GameObject chunk = Instantiate(chunkList[random], new Vector3(_spawnXPos, 0f, 0f), transform.rotation);
            ChunkController chunkController = chunk.GetComponent<ChunkController>();
            chunkController.StartRun();
            chunkController.Pool = _pool;

            // return chunk's controller
            return chunkController;
        }
        // there are no chunks in list so return an error
        else
        {
            Debug.LogError("ERROR: No chunks given to terrain manager");
            return null;
        }
    }

    // set requested chunk active
    private void OnTakeFromPool(ChunkController chunk)
    {
        chunk.gameObject.SetActive(true);
    }

    // set requested chunk inactive
    // 1 in 3 random chance to replace chunk to make sure we keep randomizing the used chunks
    private void OnReturnedFromPool(ChunkController chunk)
    {
        chunk.gameObject.SetActive(false);

        // randomly replace chunk
        // also replace chunk if it has consumables
        int random = Random.Range(0, 3);
        if (random == 0 || chunk.consumable)
        {
            // destry this chunk
            OnDestroyPoolObject(chunk);

            // replace it with a fresh chunk so the pool doesn't kill itself trying to access a dead chunk
            var newChunk = CreatedPooledItem();
            _pool.Release(newChunk);
        }
    }

    // destroy requested chunk
    private void OnDestroyPoolObject(ChunkController chunk)
    {
        Destroy(chunk.gameObject);
    }

    // get a random chunk from object pool and spawn it in line
    public void SpawnChunk()
    {
        // grab a chunk from the pool and place it in line
        var chunk = Pool.Get();
        chunk.transform.position = new Vector3(_spawnXPos, 0f, 0f);
        _spawnXPos += 10f;
    }
}