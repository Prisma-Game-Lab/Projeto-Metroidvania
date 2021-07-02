using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyJump : EnemyMovement
{
    public float jumpForce;
    private CircleCollider2D _enemyCollider;
    private Animator _animator;
    
    void Start()
    {
        base.Start();
        _enemyCollider = gameObject.GetComponent<CircleCollider2D>();
        _animator = gameObject.GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (CheckGround())
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        _animator.SetTrigger("Jumped");
    }

    private bool CheckGround()
    {
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - sp.bounds.size.y * 0.5f);
        LayerMask layers = LayerMask.GetMask( "Floor");
        //Collider2D[] hitGround = Physics2D.OverlapBoxAll(hitPosition, new Vector2(sp.bounds.size.x * 0.9f, 0.1f),0f ,layers);
        Collider2D[] hitGround = Physics2D.OverlapCircleAll(hitPosition,_enemyCollider.radius*0.9f ,layers);

        if (hitGround.Length > 0)
        {
            return true; 
        }
        
        return false;
        
    }
    
    private void OnDrawGizmosSelected()
    {
        if (sp != null)
        {
            
            Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - sp.bounds.size.y * 0.5f);
            Gizmos.DrawCube(hitPosition, new Vector2(sp.bounds.size.x * 0.9f, 0.1f));
        }
        
        
    }
}
