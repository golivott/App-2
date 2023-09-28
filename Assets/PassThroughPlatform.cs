using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    
    private LayerMask playerLayer;
    private Collider2D col;
    
    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOnPlatform() && Input.GetAxisRaw("Vertical") < 0f)
        {
            col.enabled = false;
            StartCoroutine(EnableCollision());
        }
    }

    private bool playerOnPlatform()
    {
        return Physics2D.OverlapBox(transform.position, new Vector2(col.bounds.size.x,0.1f),0, playerLayer);
    }

    private IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }
}
