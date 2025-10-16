using System.Collections;
using UnityEngine;

/// Scrip Creator: Songchalee, James
/// Recent Date Modified:
/// 10 - 16 - 2025

    

public class Tutorial : MonoBehaviour
{
    /* Get game object UI from Hierarchy */
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

            StartCoroutine("WaitForSec");

            Debug.Log("Box be hit");

        }
    }

    public IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        Destroy(uiObject);
        Destroy(gameObject);
    }


}
