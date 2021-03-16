using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Variaveis de controle
    public float jumpForce;
    public float speed;

    // Variaveis privadas 
    private Rigidbody2D _rb;
    private Vector2 _move;
    private bool _onFloor = true;

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
    public void OnPlayerJump()
    {
        if(_onFloor)
            _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        _onFloor = false; 
    }

    private void Update()
    {
        Vector2 m = _move * speed * Time.deltaTime;
        transform.Translate(new Vector2(m.x, 0f), Space.World);
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
