using System.Collections;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/7/2025
/// Handles dynamite explosion behaviour
/// </summary>

public class Explosion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Die());
    }

    // if explosion collides with an obstacle or platform, destroy it
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Platform"))
            Destroy(other.gameObject);
    }

    // wait .5 seconds then destroy explosion
    private IEnumerator Die()
    {
        yield return new WaitForSeconds(.5f);
        this.gameObject.SetActive(false);
    }
}