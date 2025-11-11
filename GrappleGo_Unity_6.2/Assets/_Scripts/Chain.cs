using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 11/11/2024
/// Handles chain behavior
/// </summary>

public class Chain : MonoBehaviour
{
    // references to player and grapple
    public Transform playerRef;
    public Transform grappleRef;

    // reference to renderer
    private SpriteRenderer _rendererRef;
    // reference to renderer's material properties
    private MaterialPropertyBlock matProperties;
    // width of the sprite
    private float _spriteWidth = 0f;

    private void OnEnable()
    {
        // get references
        _rendererRef = GetComponent<SpriteRenderer>();
        matProperties = new MaterialPropertyBlock();

        // get width of sprite
        _spriteWidth = _rendererRef.sprite.bounds.size.x;
    }

    private void Update()
    {
        // return error if don't have player and grapple refs
        if (!playerRef || !grappleRef)
            Debug.LogError("ERROR: Chain not given correct references");

        // correct z position of grapple
        Vector3 grapplPosCorrected = grappleRef.position;
        grapplPosCorrected.z = 0f;

        // get direction from player to grapple in a vector 3
        Vector3 direction = grapplPosCorrected - playerRef.position;
        // gets distance between grapple and player
        float distance = direction.magnitude;

        // set chain position to exactly in between grapple and player
        transform.position = (grapplPosCorrected + playerRef.position) / 2;
        // rotate chain
        transform.right = direction.normalized;

        // scale object to the distance between player and grapple
        transform.localScale = new Vector3(distance / _spriteWidth, transform.localScale.y, transform.localScale.z);

        // update tiling
        _rendererRef.GetPropertyBlock(matProperties);
        matProperties.SetVector("_MainTex_ST", new Vector4(distance / _spriteWidth, 1f, 0, 0));
        _rendererRef.SetPropertyBlock(matProperties);
    }
}