using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 3;
    public float jump = 1;
    public float enemyKnockback = 5;
    public bool hasBarrel = false;
    public float rollPower = 5f;
    public GameObject barrelProj;
    private GameObject barrel;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D col;
    [SerializeField] private LayerMask groundLayer = 3;
    
    private float speed;
    private Rigidbody2D rb;
    public float horizontalInput;
    public float verticalInput;
    public bool isRolling;

    private bool isAttacking = false;
    private string currentAnimation;
    private const string IDLE = "Donkey Kong Idle";
    private const string WALK = "Donkey Kong Walk";
    private const string JUMP = "Donkey Kong Jump";
    private const string ATTACK = "Donkey Kong Attack";
    private const string HAS_BARREL_IDLE = "Donkey Kong Has Barrel Idle";
    private const string HAS_BARREL_WALK = "Donkey Kong Has Barrel Walk";
    private const string HAS_BARREL_ATTACK = "Donkey Kong Has Barrel Attack";

    public UIDocument pauseMenu;
    private UIDocument playerUI;
    public float time;
    private Label timer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = rb.velocity;
        pauseMenu.enabled = false;
        playerUI = GetComponent<UIDocument>();
        timer = playerUI.rootVisualElement.Q<Label>("Timer");
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.timeSinceLevelLoad;
        timer.text = time.ToString();
        
        if (Input.GetButtonDown("Pause"))
        {
            pauseMenu.enabled = !pauseMenu.enabled;
            Time.timeScale = pauseMenu.enabled ? 0 : 1;
        }

        if (!pauseMenu.enabled)
        {
            AnimationController();
            
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            
            if (hasBarrel)
            {
                if (barrel == null)
                {
                    barrel = Instantiate(barrelProj);
                }

                if (barrel.GetComponent<BarrelProjectile>().isBroken) hasBarrel = false;
                
                barrel.transform.position = transform.position + new Vector3(0, col.bounds.size.y + barrel.GetComponent<Collider2D>().bounds.size.y/2f - 0.04f);

                // Barrel Throw
                if (!isAttacking && Input.GetButton("Attack"))
                {
                    StartCoroutine(Attack());
                    hasBarrel = false;
                    barrel.GetComponent<BarrelProjectile>().isThrown = true;
                    barrel.GetComponent<Rigidbody2D>().isKinematic = false;
                    barrel.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x * 5f,0f) , ForceMode2D.Impulse);
                }
            }
            else
            {
                barrel = null;
                
                // Roll
                if (!isAttacking && isGrounded() && Input.GetButton("Attack"))
                {
                    StartCoroutine(Attack());
                    rb.AddForce(Vector2.right * transform.localScale.x * rollPower, ForceMode2D.Impulse);
                }
            }
            
            if (isGrounded() && verticalInput > 0f)
            {
                rb.AddForce(jump * Vector2.up, ForceMode2D.Impulse);
            }

            // Prevent player from climbing up slopes
            Collider2D[] touchingCols = Physics2D.OverlapBoxAll(new Vector2(transform.position.x,transform.position.y +col.bounds.size.y/2),
                new Vector2(col.bounds.size.x + 0.02f, col.bounds.size.y), 0, groundLayer);
            foreach (Collider2D touchedCol in touchingCols)
            {
                float angle = Math.Abs(touchedCol.transform.eulerAngles.z);
                float xDirToCol = Mathf.Sign(touchedCol.transform.position.x - transform.position.x);
                if (angle >= 60 && angle <= 300)
                {
                    if(xDirToCol < 0 && horizontalInput < 0) horizontalInput = 0;
                    if(xDirToCol > 0 && horizontalInput > 0) horizontalInput = 0;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (isRolling)
            {
                rb.AddForce(Vector2.right * transform.localScale.x * rollPower, ForceMode2D.Impulse);
            }
            else
            {
                float xDirToCol = Mathf.Sign(col.transform.position.x - transform.position.x);
                rb.velocity = Vector2.zero; // remove all velocity before applying knock back
                // If grounded add more force to overcome friction
                if(isGrounded()) rb.AddForce(new Vector2(enemyKnockback * -2 * xDirToCol,enemyKnockback), ForceMode2D.Impulse);
                else rb.AddForce(new Vector2(enemyKnockback * -1 * xDirToCol,enemyKnockback), ForceMode2D.Impulse);
            }
        }
    }

    private void FixedUpdate()
    {
        float targetSpeed = horizontalInput * maxSpeed;
        float speedDelta = targetSpeed - rb.velocity.x;
        float accel = 10f;
        float movement = speedDelta * accel;

        if (isRolling)
        {
            rb.totalForce = Vector2.zero;
            rb.AddForce(Vector2.right * transform.localScale.x * rollPower, ForceMode2D.Force);
        }
        else rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        
        
        
    }
    
    void AnimationController()
    {
        // Change direction
        if (!isAttacking && horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else if (!isAttacking && horizontalInput > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }

        if (!hasBarrel)
        {
            if (isGrounded())
            {
                if (!isAttacking && horizontalInput == 0 && verticalInput == 0)
                {
                    ChangeAnimation(IDLE);
                }
            
                if (!isAttacking && horizontalInput != 0)
                {
                    ChangeAnimation(WALK);
                }

                if (!isAttacking && Input.GetButton("Attack"))
                {
                    ChangeAnimation(ATTACK);
                }
            }
            else
            {
                if (!isAttacking && Mathf.Abs(rb.velocity.y) > 2f)
                {
                    ChangeAnimation(JUMP);
                }
            }
        }
        else
        {
            if (!isAttacking && horizontalInput == 0 && verticalInput == 0)
            {
                ChangeAnimation(HAS_BARREL_IDLE);
            }
            
            if (!isAttacking && horizontalInput != 0)
            {
                ChangeAnimation(HAS_BARREL_WALK);
            }

            if (!isAttacking && Input.GetButton("Attack"))
            {
                ChangeAnimation(HAS_BARREL_ATTACK);
            }
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        if (!hasBarrel) isRolling = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length-Time.deltaTime);
        isRolling = false;
        isAttacking = false;
    }
    
    private void ChangeAnimation(string newAnimation)
    {
        if (currentAnimation != newAnimation)
        {
            animator.Play(newAnimation);
            currentAnimation = newAnimation;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(col.bounds.size.x/2f,0.01f),0, groundLayer) && rb.velocity.y < 1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(col.bounds.size.x/2f,0.01f));
        Gizmos.DrawWireCube(new Vector2(transform.position.x,transform.position.y +col.bounds.size.y/2),
            new Vector2(col.bounds.size.x + 0.02f, col.bounds.size.y));
    }
}
