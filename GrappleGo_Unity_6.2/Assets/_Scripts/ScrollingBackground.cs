using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 12/2/2025
/// Constantly scrolls background left at 1/3 the speed of the environment
/// </summary>

public class ScrollingBackground : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // only do logic in run
        if (GameManager.Instance.InRun)
        {
            // move left at a third the speed of the level
            transform.position += (GameManager.Instance.currentMoveSpeed / 3) * Time.deltaTime * -Vector3.right;
            // when background moves too far left, port back to right side
            if (transform.position.x <= -47.7f)
                transform.position = new Vector3(47.7f, 5.5f, 5f);
        }
    }
}