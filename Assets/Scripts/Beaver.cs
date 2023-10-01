using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Beaver : Enemy
{
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.gameObject.transform.position.y >
                transform.position.y + GetComponent<Collider2D>().bounds.size.y * 0.5f || col.gameObject.GetComponent<PlayerController>().isRolling)
            {
                Kill();
            }
        }
    }
}
