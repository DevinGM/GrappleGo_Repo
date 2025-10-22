using System.Collections;
using TMPro;
using UnityEngine;

/// Scrip Creator: Songchalee, James
/// Recent Date Modified:
/// 10 - 16 - 2025
/// 10 - 21 - 2025

    

public class Tutorial : MonoBehaviour
{
    public float fadeTime;
    private TextMeshProUGUI fadeAwayText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeAwayText = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeTime > 0)
        {
            // How to reduce the given time.
            fadeTime -= Time.deltaTime;

            // Use that reducing time to fade Text.
            fadeAwayText.color = new Color(fadeAwayText.color.r, fadeAwayText.color.g, fadeAwayText.color.b, fadeTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
      
    }

    


}
