using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/3/2025
/// Holds player behaviours
/// </summary>

public class PlayerBehaviour : MonoBehaviour
{
    // turn on to start the run
    [Header("Turn On To Start The Run")]
    [SerializeField] private bool _StartRun = false;

    // speed player moves at
    [SerializeField] private float _moveSpeed = 10f;
    // distance travelled
    [SerializeField] private int _score = 0;
    // highest distance ever travelled
    [SerializeField] private int _highScore = 0;
    // is the player currently moving
    [SerializeField] private bool _moving = false;
    // position at the start of a run
    private Vector3 _spawnPos;

    void OnEnable()
    {
        EventBus.Subscribe(EventType.RunStart, StartRun);
        EventBus.Subscribe(EventType.RunEnd, EndRun);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(EventType.RunStart, StartRun);
        EventBus.Unsubscribe(EventType.RunEnd, EndRun);
    }

    // called when run starts
    private void StartRun()
    {
        _spawnPos = transform.position;
        _moving = true;
    }
    
    // called when run ends
    private void EndRun()
    {
        transform.position = _spawnPos;
        _moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_StartRun)
        {
            EventBus.Publish(EventType.RunStart);
            _StartRun = false;
        }

        // handle score
        _score = (int)transform.position.x;
        if (_highScore < _score)
            _highScore = _score;

        // move forward constantly
        if (_moving)
        {
            Vector3 movePos = transform.position;
            movePos.x = transform.position.x + _moveSpeed * Time.deltaTime;
            transform.position = movePos;
        }
    }

    // handles collision interactions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            EventBus.Publish(EventType.RunEnd);
        }
    }

    // temp prototyping ui
    private void OnGUI()
    {
        GUIStyle customStyle = new GUIStyle(GUI.skin.label);
        customStyle.fontSize = 30;

        Rect scoreText = new Rect(10, 10, 200, 40); // x, y, width, height
        GUI.Label(scoreText, "Score: " + _score, customStyle);
        Rect highScoreText = new Rect(10, 50, 200, 40); // x, y, width, height
        GUI.Label(highScoreText, "High Score: " + _highScore, customStyle);
    }
}