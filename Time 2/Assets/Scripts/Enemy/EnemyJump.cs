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
    private bool _alreadyJumped = false;
    
    
    void Start()
    {
        base.Start();
        _enemyCollider = gameObject.GetComponent<CircleCollider2D>();
        _animator = gameObject.GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        base.FixedUpdate();
        if (CheckGround() && !_alreadyJumped)
        {
            Jump();
            _alreadyJumped = true;
        }
        else
        {
            _alreadyJumped = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Verifica a morte pela agua
        if (collision.collider.CompareTag("Water"))
        {
            Destroy(gameObject);
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
        Collider2D[] hitGround = Physics2D.OverlapBoxAll(hitPosition, new Vector2(sp.bounds.size.x * 0.5f, 0.5f),0f ,layers);
        //Collider2D[] hitGround = Physics2D.OverlapCircleAll(hitPosition,_enemyCollider.radius*0.9f ,layers);

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
            Gizmos.DrawCube(hitPosition, new Vector2(sp.bounds.size.x * 0.5f, 0.5f));
        }
        
        
    }
}
