using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 10/14/2024
/// HANDLES DISTANCE SCORE
/// </summary>
public class ScoreGatherer : MonoBehaviour
{
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
        // reset position
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // logic only happens when in a run
        if (GameManager.Instance.InRun)
        {
            // move right constantly
            transform.Translate(GameManager.Instance.currentMoveSpeed * Time.deltaTime * transform.right);

            // add to distance score
            // gatherer MUST start at x = 0 for score to be accurate
            GameManager.Instance.distanceScore = (int)transform.position.x;
        }
    }
}