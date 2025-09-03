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

    // speed player begins moving at
    [SerializeField] private float _startSpeed = 10f;
    // speed player is moving at
    [SerializeField] private float _moveSpeed;
    // amount player accelerates by
    [SerializeField] private float _accelSpeed = 1f;
    // amount of time passed before player accelerates
    [SerializeField] private float _accelTime = 5f;
    // distance travelled
    [SerializeField] private int _score = 0;
    // highest distance ever travelled
    [SerializeField] private int _highScore = 0;
    // number of player's lives
    // player dies when _lives hits 0
    [SerializeField] private int _lives = 1;
    // is the player currently in a run
    [SerializeField] private bool _inRun = false;
    // is the player currently moving
    [SerializeField] private bool _moving = false;

    // position at the start of a run
    private Vector3 _spawnPos;
    // is the player currently moving
    private bool _waitingToAccelerate = false;

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
        _inRun = true;
        _moveSpeed = _startSpeed;
    }
    
    // called when run ends
    private void EndRun()
    {
        transform.position = _spawnPos;
        _moving = false;
        _inRun = false;
        _lives = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // start a run when _StartRun is set to true
        // mainly dev tool, most likely remove for build
        if (_StartRun)
        {
            EventBus.Publish(EventType.RunStart);
            _StartRun = false;
        }
        
        // logic only happens when in a run
        if (_inRun)
        {
            // increase speed by 1 every 5 seconds
            if (!_waitingToAccelerate)
            {
                StartCoroutine(Accelerate());
            }

            // handle score
            // player MUST start run at x = 0 for score to be accurate
            _score = (int)transform.position.x;
            if (_highScore < _score)
                _highScore = _score;

            // move forward constantly
            if (_moving)
                transform.position = new Vector3(transform.position.x + _moveSpeed * Time.deltaTime, 
                    transform.position.y, transform.position.z);

            // if the player runs out of lives end the run
            if (_lives <= 0)
                EventBus.Publish(EventType.RunEnd);
        }
    }

    // handles collision interactions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            _lives--;
    }

    // wait _accelTime amount of seconds before increasing speed by _accelSpeed
    private IEnumerator Accelerate()
    {
        _waitingToAccelerate = true;
        yield return new WaitForSeconds(_accelTime);
        _moveSpeed += _accelSpeed;
        _waitingToAccelerate = false;
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