using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 10/29/2025
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
    //    EventBus.Subscribe(EventType.DashPerformed, OnDash);

        // get rigidbody
        _rbRef = GetComponent<Rigidbody>();
    }
    void OnDisable()
    {
        // unsubscribe to events
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    //    EventBus.Unsubscribe(EventType.DashPerformed, OnDash);
    }

    // called when run ends
    private void EndRun()
    {
        // reset position for cleanliness
        transform.position = Vector3.zero;
        _rbRef.linearVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // only do logic during run
        if (GameManager.Instance.InRun)
        {
            // move left constantly
            transform.Translate(GameManager.Instance.currentMoveSpeed * Time.fixedDeltaTime * -transform.right);
            
            if (_rbRef.linearVelocity.magnitude > 0)
            {
                _rbRef.linearVelocity += new Vector3(1f, 0f, 0f);
                if (_rbRef.linearVelocity.x > 0f)
                {
                    _rbRef.linearVelocity = Vector3.zero;
                }
            }
        }
    }

    // called when player dashes
    private void OnDash()
    {
        // reset force
        _rbRef.linearVelocity = Vector3.zero;
        // add new force
        _rbRef.AddForce(-transform.right * GameManager.Instance.dashForce, ForceMode.Impulse);
    }
}