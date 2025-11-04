using System.Collections;
using TMPro;
using UnityEngine;

/// Scrip Creator: Songchalee, James
/// Recent Date Modified:
/// 10 - 16 - 2025
/// 10 - 21 - 2025

    

public class FadingText : MonoBehaviour
{
    public float fadeTime;
    private TextMeshProUGUI fadeAwayText;
    private float alphaValue;
    private float fadeAwayPerSecond;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeAwayText = GetComponent<TextMeshProUGUI>();
        
        // This will determine how many per second would fadeTime will remove it's color.
        fadeAwayPerSecond = 1 / fadeTime;

        // For a better fade, use another variable we will call "alphaValue". 
        alphaValue = fadeAwayText.color.a;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeTime > 0)
        {
            // How to reduce the given time.
            fadeTime -= Time.deltaTime;

            // Reduce color of text more smoothly. 
            alphaValue -= fadeAwayPerSecond * Time.deltaTime;

            // Use that reducing time to fade Text.
            fadeAwayText.color = new Color(fadeAwayText.color.r, fadeAwayText.color.g, fadeAwayText.color.b, alphaValue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }

}
