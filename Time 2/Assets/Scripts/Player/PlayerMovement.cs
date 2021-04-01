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
    public float flightGravity;
    public LayerMask groundMask;

    // Variaveis privadas 
    private Rigidbody2D _rb;
    private float _playerGravity;
    private Vector2 _move;
    [HideInInspector] public bool isFlipped = false;
    private bool _jumpHold = false;
    private bool _jumpbreak = false;
    private SpriteRenderer _sr;
    private PlayerStatus _playerStatus;
    private Collider2D _collider2D;

    
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _playerGravity = _rb.gravityScale; // gravidade original do player
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _collider2D = gameObject.GetComponent<Collider2D>();
    }

        // Esse evento é chamado quando o jogador mexe nos inputs de movimento 
    public void OnPlayerMove(InputAction.CallbackContext ctx)
    {
        _move = ctx.ReadValue<Vector2>();
    }

    // Esse evento é chamado quando o jogador aperta o botão de pulo 
    public void OnPlayerJump(InputAction.CallbackContext ctx)
    {
        // if (playerState == PlayerSkill.BoatMode)//Nao pula quando se for barco
        //     return;

        if (IsGrounded() && ctx.started)
        {
            _jumpHold = true;
            _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        if (gameObject.GetComponent<PlaneSkill>().obtained && !IsGrounded() && ctx.started)//se o player tiver o  aviao, ele flutua quando o jogador segura o botao de pulo com estando no ar
        {
            _playerStatus.playerState = PlayerSkill.PlaneMode;
            _sr.color = Color.yellow;
            _jumpHold = true;
            _rb.gravityScale = flightGravity;
        }

        if (_jumpHold && ctx.canceled)
        {
            _rb.gravityScale = _playerGravity;//corrige a gravidade quando o jogador solta o botao de pulaa
            if (_playerStatus.playerState == PlayerSkill.PlaneMode)
            {
                _playerStatus.playerState = PlayerSkill.Normal;
                _sr.color = Color.white;
            }
            
            if (_rb.velocity.y > 0f)
                _jumpbreak = true;
            
            _jumpHold = false;
        }

    }

    private void FixedUpdate()
    {
  
        Vector2 m = _move * (speed * Time.fixedDeltaTime);
        _rb.velocity = (new Vector2(m.x * speed, _rb.velocity.y));
        Flip();

        FlightBreak();
        BoatBreak();

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

    private void FlightBreak()
    {
        if (IsGrounded() && _playerStatus.playerState == PlayerSkill.PlaneMode)//verificacao para quando o player retorna ao chao, depois de planar
        {
            _rb.gravityScale = 1f;//corrige a gravidade quando o aviao toca o chao
            _playerStatus.playerState = PlayerSkill.Normal;//corrige a forma do player
            _sr.color = Color.white;
        }
    }
    
    private void BoatBreak()
    { 
        // O barco anda na agua
        if(OnWater())
            return;
        
        if (IsGrounded() && _playerStatus.playerState == PlayerSkill.BoatMode)//verificacao para quando o player retorna ao chao, depois de planar
        {
            _rb.velocity = _rb.velocity = new Vector2(0f, _rb.velocity.y); 
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
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - _collider2D.bounds.size.y/2);
        LayerMask layers = groundMask;

        if (_playerStatus.playerState == PlayerSkill.BoatMode)
        {
            layers = LayerMask.GetMask("BoatFloor", "Floor");
        }
        Collider2D[] hitGround = Physics2D.OverlapCircleAll(hitPosition, detectGroundRange, layers);

        if (hitGround.Length > 0)
        {
            return true; 
        }
        
        return false;
    }

    private bool OnWater()
    {
        
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - _collider2D.bounds.size.y/2);
        LayerMask waterLayer = LayerMask.GetMask("BoatFloor");
        Collider2D[] hitGround = Physics2D.OverlapCircleAll(hitPosition, detectGroundRange, waterLayer);

        if (hitGround.Length > 0)
        {
            return true; 
        }
        
        return false;
    }

    private void OnDrawGizmosSelected()
    {

        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y/2);

        Gizmos.DrawWireSphere(hitPosition, detectGroundRange);
    }


}
