using System.Collections;
using UnityEngine;

/// Scrip Creator: Songchalee, James
/// Recent Date Modified:
/// 10 - 21 - 2025

public class StartTutorial : MonoBehaviour
{

    public GameObject uiObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            uiObject.SetActive(false);
            //StartCoroutine("WaitForSec");

        }
    }

   /* IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        Destroy(uiObject);
        Destroy(gameObject);
    }*/

}
