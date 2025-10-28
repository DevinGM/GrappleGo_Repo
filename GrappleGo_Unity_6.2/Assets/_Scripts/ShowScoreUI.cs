using UnityEngine;

/// Script Creator: James Songchalee
/// Recent Date Modified:
/// 10 - 28 - 2025
/// 

public class ShowScoreUI : MonoBehaviour

{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.highScore = 0;
        
    }
}
