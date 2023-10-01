using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed;
    public float moveDist;
    private Rigidbody2D rb;
    private Vector3 startPos;
    
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public Vector3 playerDetection = new Vector3(3, 10);

    public enum MoveDir
    {
        Left,
        Right,
        Stop
    }

    public MoveDir currMoveDir;

    [ExecuteAlways]
    private void OnValidate()
    {
        startPos = transform.position;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        currMoveDir = MoveDir.Stop;
    }

    private void Update()
    {
        // Change direction
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
    }

    private void FixedUpdate()
    {
        Collider2D playerCol = Physics2D.OverlapBox(startPos + new Vector3(-moveDist/2f,0), playerDetection + new Vector3(moveDist, 0), 0, playerLayer);
        if (playerCol == null && !GetComponent<Enemy>().isDead)
        {
            currMoveDir = MoveDir.Stop;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            transform.position = startPos;
        }

        if (playerCol != null && currMoveDir == MoveDir.Stop && !GetComponent<Enemy>().isDead)
        {
            currMoveDir = MoveDir.Left;
            rb.isKinematic = false;
        }
        
        if (currMoveDir != MoveDir.Stop)
        {
            if (transform.position.x <= startPos.x - moveDist) currMoveDir = MoveDir.Right;
            else if(transform.position.x >= startPos.x) currMoveDir = MoveDir.Left;
        
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
            Vector2 movement = new Vector2(rayHit.normal.y,rayHit.normal.x);

            if (currMoveDir == MoveDir.Left)
            {
                movement.x = -speed;
                movement.y *= speed;
            }
            else if(currMoveDir == MoveDir.Right)
            {
                movement.x = speed;
                movement.y *= -speed;
            }
        
            if (rayHit.collider == null) movement.y = -1f;
        
            rb.velocity = movement;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (currMoveDir == MoveDir.Left) currMoveDir = MoveDir.Right;
            else if (currMoveDir == MoveDir.Right) currMoveDir = MoveDir.Left;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(startPos.x,transform.position.y), new Vector3(startPos.x - moveDist,transform.position.y));
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.1f);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(startPos + new Vector3(-moveDist/2f,0),playerDetection + new Vector3(moveDist,0));
    }
}
