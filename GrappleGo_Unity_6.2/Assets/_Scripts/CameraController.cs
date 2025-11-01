using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 10/23/2025
/// Holds camera movement
/// </summary>

public class CameraController : MonoBehaviour
{
    // camera starting position
    private Vector3 _spawnPos;
    
    void OnEnable()
    {
        _spawnPos = transform.position;

        // subscribe to events
        EventBus.Subscribe(EventType.RunEnd, EndRun);
    }
    
    void OnDisable()
    {
        // unsubsribe to events
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called on run end
    private void EndRun()
    {
        transform.position = _spawnPos;
    }
    
    // Update is called once per frame
    void Update()
    {
        // move camera with player speed
      //  if (GameManager.Instance.InRun)
           // transform.Translate(PlayerController.Instance.currentMoveSpeed * Time.deltaTime * transform.right);
    }
}