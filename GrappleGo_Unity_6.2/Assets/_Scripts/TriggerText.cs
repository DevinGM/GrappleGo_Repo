using System.Collections;
using UnityEngine;
using TMPro;

/// Scrip Creator: Songchalee, James
/// Recent Date Modified:
/// 10 - 21 - 2025
/// 10 - 22 - 2025

/* Notes: Attatch this script to the gameObject that the Player would collide with. */

public class TriggerText : MonoBehaviour
{
    // Get the selected gameObject, in this case, the Text under Tutorial Canvas. 
    public GameObject textObject;

    // The desired time for text to vanish during gameplay.
    public float fadeTime;

    // Bool to determine when Text is active.
    private bool textActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // To debug and let any developer know that text is currently false.
        Debug.Log("text is now false");

        // To make the textObject be false at the start of the gameplay.
        textObject.SetActive(false);

        // To make "bool textActive" flase at the start of the gameplay.
        textActive = false;
    }

    // Update is called once per frame
    void Update()
    {

        // This is a countdown for the textObject that will eventually vanish during gameplay.

        if (textActive == true)
        {
            // If fadeTime is LESS than 0, make fadeTime countdown by subtracting with Time.deltaTime.
            if (fadeTime > 0)
            {
                fadeTime -= Time.deltaTime;
            }
            else if (fadeTime > 0){

                // A debug just in case if fadeTime subtracting with Time.deltaTime goes wrong.
                Debug.Log("It's not counting down.");
            }

            // This is the results when fadeTime becomes LESS than or EQUAL to zero. 

            if (fadeTime <= 0)
            {
                textObject.SetActive(false);
                Debug.Log("Count down complete.");

                // This will also destory the trigger object so that the player would not trigger it again after respawn.
                Destroy(gameObject);
            }
        }
        
    }

    // The trigger object that the player would collide with.

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Makes textActive become true.
            textActive = true;

            // Debug for testing if the player collides with the trigger object.
            Debug.Log("Trigger Message");
            
            // Makes the textObject become true which also displays text during gameplay.
            textObject.SetActive(true);

            // Debug to ensure that the textObject becomes active and gets displayed during gameplay.
            Debug.Log("text is now true");
        }
           
    }


    

}
