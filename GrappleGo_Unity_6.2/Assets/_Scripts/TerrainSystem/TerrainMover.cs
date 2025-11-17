using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/11/2025
/// Handles moving terrain
/// </summary>

public class TerrainMover : MonoBehaviour
{
    // reference to rigidbody
    private Rigidbody _rbRef;

    void OnEnable()
    {
        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, EndRun);
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called when run ends
    private void EndRun()
    {
        // reset position for cleanliness
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // only do logic during run
        if (GameManager.Instance.InRun)
        {
            // move left constantly
            transform.Translate(GameManager.Instance.currentMoveSpeed * Time.fixedDeltaTime * -transform.right);
        }
    }
}