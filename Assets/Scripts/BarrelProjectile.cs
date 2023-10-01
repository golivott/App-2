using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col2D;
    private Animator animator;
    public bool isBroken;
    public bool isThrown;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rb.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(OnDeath());
            col.gameObject.GetComponent<Enemy>().Kill();
        }

        if (col.gameObject.CompareTag("Floor") && isThrown)
        {
            float angle = col.gameObject.transform.eulerAngles.z;
            if (angle >= 60 && angle <= 300)
            {
                StartCoroutine(OnDeath());
            }
        }
    }

    private IEnumerator OnDeath()
    {
        isBroken = true;
        animator.Play("Death");
        rb.freezeRotation = true;
        rb.isKinematic = true;
        transform.position += new Vector3(0, col2D.bounds.size.y / 2f, 0);
        yield return new WaitForEndOfFrame();
        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSecondsRealtime(length);
        Destroy(gameObject);
    }
}
