using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Variaveis de controle
    public float jumpForce;
    public float speed;
    public float holdTime;

    // Variaveis privadas 
    private Rigidbody2D _rb;
    private Vector2 _move;
    private bool _onFloor = true;
    [HideInInspector] public bool isFlipped = false;
    private bool _jumpHold = false;
    private float _jumpProgression = 0f;


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
        Debug.Log(ctx.started);
        
        if (_onFloor && ctx.started)
        {
            _jumpHold = true;
            _jumpProgression = 0f;
        }

        if(_jumpHold && ctx.canceled)
        {
            _rb.AddForce(new Vector2(0, jumpForce*(_jumpProgression/holdTime)), ForceMode2D.Impulse);
            _jumpHold = false;
            _onFloor = false;
        }

        //Debug.Log("Pulou!");
         
    }

    private void FixedUpdate()
    {
        Vector2 m = _move * speed * Time.fixedDeltaTime;
        _rb.velocity = (new Vector2(m.x * speed, _rb.velocity.y));
        Flip();
        //transform.Translate(new Vector2(m.x, 0f), Space.World);

        if (_jumpHold)
        {
            _jumpProgression += Time.fixedDeltaTime;
            if (_jumpProgression >= holdTime)
            {
                _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                _jumpHold = false;
                _onFloor = false;
            }
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _onFloor = true;
        }
    }

}
