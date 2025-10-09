using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Script Creator: Songchalee, James
/// Recent Date Modified: 
/// 10 - 07 - 2025

public class Trigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Box be hit");
        }
    }



}
