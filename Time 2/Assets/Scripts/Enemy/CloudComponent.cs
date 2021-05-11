using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CloudComponent : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    [Header("Esse e a variavel que controla o quanto a nuvem sobe ou desce")]
    public float cloudEffect;
    
    [Header("Esse e a variavel que recebe o inimigo")]
    public EnemyMovement enemyBrain;
    
    private bool _playerSteped;
    public Rigidbody2D rb;
    private float _originalYPos;

    void Start()
    {
        rb.gravityScale = 0;
        _playerSteped = false;
        _originalYPos = enemyBrain.gameObject.transform.position.y;
    }

    // Update is called once per frame
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStatus playerStatus = other.gameObject.GetComponent<PlayerStatus>();
            Rigidbody2D playerRb = playerStatus.rb;
            if (enemyBrain.enemyMovement == SimpleEnemyMovements.Horizontal)
            {
                float xVelocity = rb.velocity.x;
                if(playerStatus.playerAnimationState == PlayerAnimationState.Idle)
                    playerRb.velocity = new Vector2(xVelocity, playerRb.velocity.y);
                else if(playerStatus.playerAnimationState == PlayerAnimationState.Movement)
                    playerRb.velocity = new Vector2(xVelocity - playerRb.velocity.x, playerRb.velocity.y);
               
                
            }else if (enemyBrain.enemyMovement == SimpleEnemyMovements.Vertical)
            {
                float yVelocity = rb.velocity.y;
                playerRb.velocity = new Vector2(playerRb.velocity.x, yVelocity);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Mover um pouco para baixo
            if (PlayerInRange() && !_playerSteped)
            {
                rb.AddForce(Vector2.down* cloudEffect, ForceMode2D.Impulse);
                _playerSteped = true;
            }
            
        }
    }

    private IEnumerator PlayerSteped()
    {
        while (enemyBrain.gameObject.transform.position.y < _originalYPos)
        {
            Vector3 pos = enemyBrain.gameObject.transform.position;
            enemyBrain.gameObject.transform.position = new Vector3(pos.x, pos.y + 0.01f*0.5f, pos.z);
            yield return new WaitForEndOfFrame();
        }

        _playerSteped = false;
    }

    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         // Mover um pouco para cima
    //         
    //        
    //     }
    // }

    private void Update()
    {
        if(_playerSteped && !PlayerInRange())
            StartCoroutine(PlayerSteped());
    }
    
    private bool PlayerInRange()
    {
        LayerMask layer = LayerMask.GetMask( "Player");
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        //Collider2D[] hitWall = Physics2D.OverlapCircleAll(transform.position, 1f, layer);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + collider.size.y, transform.position.z);
        Collider2D[] hitGround = Physics2D.OverlapBoxAll(pos, new Vector2(collider.bounds.size.x, collider.bounds.size.y*1.5f),0f ,layer);
        // Verificar se o player esta no range 
        if (hitGround.Length > 0)
        {
            return true; 
        }
        
        return false;
        
    }
    //
    // private void OnDrawGizmosSelected()
    // {
    //     if (gameObject.GetComponent<BoxCollider2D>() != null)
    //     {
    //
    //         BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
    //         Vector3 pos = new Vector3(transform.position.x, transform.position.y + collider.size.y,
    //             transform.position.z);
    //         // Gizmos.DrawCube(hitPosition, new Vector2(_playerStatus.playerCollider.size.x, 0.1f));
    //         // Gizmos.DrawCube(hitPosition2, new Vector2(0.1f,_playerStatus.playerCollider.size.y * 0.2f));
    //         // Gizmos.DrawCube(hitPosition2, new Vector2(0.1f,_playerStatus.playerCollider.size.y * 0.2f));
    //         Gizmos.DrawCube(pos, new Vector2(collider.bounds.size.x , collider.size.y*1.5f));
    //     }
    //     
    //     
    // }
    
}
