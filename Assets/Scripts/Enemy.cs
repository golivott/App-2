using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private string currentAnimation;
    private Rigidbody2D rb;
    private Animator animator;
    public LayerMask player;
    public bool isDead;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ChangeAnimation("Walk");
    }

    private void Update()
    {
        if(transform.position.y < -5) Destroy(gameObject);
    }

    public void Kill()
    {
        isDead = true;
        rb.excludeLayers = player;
        EnemyMove enemyMove = GetComponent<EnemyMove>();
        if (enemyMove != null)
        {
            enemyMove.currMoveDir = EnemyMove.MoveDir.Stop;
            rb.velocity = Vector2.zero;
        }
        StartCoroutine(OnDeath());
    }

    private IEnumerator OnDeath()
    {
        
        ChangeAnimation("Death");
        yield return new WaitForSecondsRealtime(0.05f);
        rb.AddForce(4f * Vector2.up, ForceMode2D.Impulse);
        yield return new WaitForEndOfFrame();
        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSecondsRealtime(length);
        Destroy(gameObject);
    }

    private void ChangeAnimation(string newAnimation)
    {
        if (currentAnimation != newAnimation)
        {
            animator.Play(newAnimation);
            currentAnimation = newAnimation;
        }
    }
}
