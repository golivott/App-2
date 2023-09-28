using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 3;
    public float jump = 1;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Collider2D col;
    [SerializeField] private LayerMask groundLayer = 3;
    
    private float speed;
    private Rigidbody2D rb;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 lastVelocity;
    
    MaterialPropertyBlock mpb;
    public MaterialPropertyBlock Mpb
    {
        get
        {
            if (mpb == null)
            {
                mpb = new MaterialPropertyBlock();
            }

            return mpb;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
        
        if (isGrounded() && _verticalInput > 0f)
        {
            rb.AddForce(jump*Vector2.up, ForceMode2D.Impulse);
        }

        Collider2D[] touchingCols = Physics2D.OverlapBoxAll(new Vector2(transform.position.x,transform.position.y +col.bounds.size.y/2),
            new Vector2(col.bounds.size.x + 0.02f, col.bounds.size.y), 0, groundLayer);
        foreach (Collider2D touchedCol in touchingCols)
        {
            float angle = Math.Abs(touchedCol.transform.eulerAngles.z);
            float xDirToCol = Mathf.Sign(touchedCol.transform.position.x - transform.position.x);
            if (angle >= 60 && angle <= 300)
            {
                if(xDirToCol < 0 && _horizontalInput < 0) _horizontalInput = 0;
                if(xDirToCol > 0 && _horizontalInput > 0) _horizontalInput = 0;
            }
        }
        lastVelocity = rb.velocity;

        // RaycastHit2D leftRayHit = Physics2D.Raycast(transform.position, Vector3.left, col.bounds.size.x/2+0.01f, groundLayer);
        // if (leftRayHit.collider != null)
        // {
        //     float angle = Vector2.Angle(leftRayHit.normal, Vector2.up);
        //     if (angle >= 60 && angle <= 90 && _horizontalInput < 0)
        //         _horizontalInput = 0;
        // }
        //
        // RaycastHit2D rightRayHit = Physics2D.Raycast(transform.position, Vector3.right, col.bounds.size.x/2+0.01f, groundLayer);
        // if (rightRayHit.collider != null)
        // {
        //     float angle = Vector2.Angle(rightRayHit.normal, Vector2.up);
        //     if (angle >= 60 && angle < 90 && _horizontalInput > 0 && _verticalInput <= 0)
        //         _horizontalInput = 0;
        // }
    }

    private void FixedUpdate()
    {
        float targetSpeed = _horizontalInput * maxSpeed;
        float speedDelta = targetSpeed - rb.velocity.x;
        float accel = 10f;
        float movement = speedDelta * accel;
        
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(col.bounds.size.x/2,0.01f),0, groundLayer) && rb.velocity.y < 1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(col.bounds.size.x/2,0.01f));
        Gizmos.DrawWireCube(new Vector2(transform.position.x,transform.position.y +col.bounds.size.y/2),
            new Vector2(col.bounds.size.x + 0.02f, col.bounds.size.y));
        // Gizmos.DrawRay(transform.position,Vector3.left * (col.bounds.size.x/2f + 0.01f));
        // Gizmos.DrawRay(transform.position,Vector3.right * (col.bounds.size.x/2f + 0.01f));
    }
}
