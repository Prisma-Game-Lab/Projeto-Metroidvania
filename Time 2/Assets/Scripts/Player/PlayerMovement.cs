using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Variaveis de controle
    public float jumpForce;
    public float jumpDecreaseRate;
    public float speed;
    public float holdTime;
    public float detectGroundRange;
    public LayerMask groundMask;

    // Variaveis privadas 
    private Rigidbody2D _rb;
    private Vector2 _move;
    [HideInInspector] public bool isFlipped = false;
    private bool _jumpHold = false;
    private bool _jumpbreak = false;


    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Esse evento é chamado quando o jogador mexe nos inputs de movimento 
    public void OnPlayerMove(InputAction.CallbackContext ctx)
    {
        _move = ctx.ReadValue<Vector2>();
    }

    // Esse evento é chamado quando o jogador aperta o botão de pulo 
    public void OnPlayerJump(InputAction.CallbackContext ctx)
    {
        PlayerSkill playerState = gameObject.GetComponent<PlayerStatus>().playerState;

        if (playerState == PlayerSkill.BoatMode)//Nao pula quando se for barco
            return;
        //Debug.Log(ctx.started);
        
        if (IsGrounded() && ctx.started)
        {
            _jumpHold = true;
            _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        if(_jumpHold && ctx.canceled)
        {
            if (_rb.velocity.y > 0f)
                _jumpbreak = true;
            
            _jumpHold = false;
        }

        //Debug.Log("Pulou!");
         
    }

    private void FixedUpdate()
    {
        Vector2 m = _move * (speed * Time.fixedDeltaTime);
        _rb.velocity = (new Vector2(m.x * speed, _rb.velocity.y));
        Flip();
        //transform.Translate(new Vector2(m.x, 0f), Space.World);

        if (_jumpbreak)
        {
            float yVelocity = _rb.velocity.y;
            if ( yVelocity > 0f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, yVelocity - jumpDecreaseRate);
            }
            else
                _jumpbreak = false;
        }
    }

    // flipar no eixo x quando o player muda de direção 
    private void Flip()
    {
        // movendo para a esquerda não flipado. Vai flipar 
        if (!isFlipped && _move.x < 0)
        {
            Vector3 newLocalScale = transform.localScale;
            newLocalScale.x *= -1;
            transform.localScale = newLocalScale;
            isFlipped = true;
        }

        // movendo para a direta flipado. Vai flipar 
        if (isFlipped && _move.x > 0)
        {
            Vector3 newLocalScale = transform.localScale;
            newLocalScale.x *= -1;
            transform.localScale = newLocalScale;
            isFlipped = false;
        }
            
    }

    // Detectando colisão com o chão 
    
    private bool IsGrounded()
    {
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y/2);
        Collider2D[] hitGround = Physics2D.OverlapCircleAll(hitPosition, detectGroundRange, groundMask);
        
        foreach (Collider2D ground in hitGround)
        {
            if (ground != null)
                return true;
        }

        return false;


    }

    private void OnDrawGizmosSelected()
    {

        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y/2);
        if (hitPosition == null)
            return;

        Gizmos.DrawWireSphere(hitPosition, detectGroundRange);
    }


}