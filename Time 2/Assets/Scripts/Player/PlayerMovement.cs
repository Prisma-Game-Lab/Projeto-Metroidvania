using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Variaveis de controle
    public float jumpForce;
    public float wallJumpForce;
    public float jumpDecreaseRate;
    public float ballVelocityThreshold;
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
    private Transform _playerTransform;
    private Vector3 _originalLocalScale;
    private SpriteRenderer _sr;
    private PlayerStatus _playerStatus;
    private BoxCollider2D _collider2D;
    private PlaneSkill _planeSkill;
    private ParticleSystem _trasformationParticles;

    
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _playerGravity = _rb.gravityScale; // gravidade original do player
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _collider2D = gameObject.GetComponent<BoxCollider2D>();
        _planeSkill = gameObject.GetComponent<PlaneSkill>();
        _playerTransform = transform;
        _originalLocalScale = _playerTransform.localScale;
        _trasformationParticles = _playerStatus.transformationParticles.GetComponent<ParticleSystem>();
        
    }

        // Esse evento é chamado quando o jogador mexe nos inputs de movimento 
    public void OnPlayerMove(InputAction.CallbackContext ctx)
    {
        _move = ctx.ReadValue<Vector2>();
    }

    // Esse evento é chamado quando o jogador aperta o botão de pulo 
    public void OnPlayerJump(InputAction.CallbackContext ctx)
    {
        // comecou a pressionar o butao
        if (ctx.started)
        {
            
            if (_playerStatus.playerState == PlayerSkill.ShurikenMode)
            {
                if(CheckWall() && !IsGrounded())
                {
                   
                    _rb.gravityScale = _playerGravity;
                    // Pulo na diagonal realizado pela shuriken 
                    if (isFlipped)
                    {
                        _rb.AddForce(new Vector2(5*wallJumpForce, wallJumpForce), ForceMode2D.Impulse);
                        
                    }
                    else
                    {
                        _rb.AddForce(new Vector2(-5*wallJumpForce, wallJumpForce), ForceMode2D.Impulse);
                        
                    }
                }
            }

            if (IsGrounded())
            {
                AudioManager.instance.Play("Jump");
                _jumpHold = true;
                _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
            
            Flight();
        }


        if (_jumpHold && ctx.canceled)
        {
           
            if (_playerStatus.playerState == PlayerSkill.PlaneMode)
            {
                _trasformationParticles.Play();//liga particulas de transformacao
                AudioManager.instance.Play("Transform");
                _playerStatus.playerState = PlayerSkill.Normal;
                
                //_sr.color = Color.white;
                _rb.gravityScale = _playerGravity;//corrige a gravidade quando o jogador solta o botao de pulaa
                _sr.sprite = _playerStatus.normalSprite;
                Vector3 v = _playerStatus.sr.bounds.size;
                BoxCollider2D b = _playerStatus.collider as BoxCollider2D;
                b.size = v;
            }
            
            if (_rb.velocity.y > 0f)
                _jumpbreak = true;
            
            _jumpHold = false;
        }

    }

    // funcao que faz a logica do voo
    private void Flight()
    {
        //Se o player tiver o aviao, ele flutua quando o jogador segura o botao de pulo no ar. E NAO PODE SER USADO ENQUANTO SHURIKEN NA PAREDE 
        if (_planeSkill.obtained && !IsGrounded() && !(_playerStatus.playerState == PlayerSkill.ShurikenMode && CheckWall()))
        {
            _playerStatus.playerState = PlayerSkill.PlaneMode;
            
            //manter o flip 
            // flip tem que se manter 
            if (isFlipped)
            {
                Vector3 flippedScale = _originalLocalScale;
                flippedScale.x *= -1f;
                _playerTransform.localScale = flippedScale;
            }
            else
            {
                _playerTransform.localScale = _originalLocalScale;
            }
            AudioManager.instance.Play("Transform");
            _trasformationParticles.Play();//liga particulas de transformacao
            //_sr.color = Color.yellow;
            _jumpHold = true;
            _rb.gravityScale = flightGravity;
            _sr.sprite = _planeSkill.planeSprite;
            Vector3 v = _playerStatus.sr.bounds.size;
            BoxCollider2D b = _playerStatus.collider as BoxCollider2D;
            b.size = v;
              
            // 
            if (_rb.velocity.y < -10f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                _rb.AddForce(new Vector2(0f, _rb.velocity.y *0.1f),ForceMode2D.Impulse);
            }
                    
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_playerStatus.playerState == PlayerSkill.ShurikenMode)
        {
            if (CheckWall() && !IsGrounded())
            {
                _rb.gravityScale = 0f;
                _rb.velocity = new Vector2(_rb.velocity.x,0f);
            }
            else
            {
                _rb.gravityScale = _playerGravity;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!(_playerStatus.playerState == PlayerSkill.ShurikenMode && CheckWall()) )
        {
            Vector2 m = _move * (speed * Time.fixedDeltaTime);
            _rb.velocity = (new Vector2(m.x * speed, _rb.velocity.y));
        }
        
        if (_playerStatus.playerState == PlayerSkill.ShurikenMode)
        {
            if (!CheckWall())
            {
                _rb.gravityScale = _playerGravity;
            }
        }

        Flip(); 
        BallBreak();
        if (IsGrounded())
        {
            SaveSafePosition(); // Save Safe position for the player 
            FlightBreak();
            BoatBreak();
            ShurikenBreak();
        }

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
        if (_playerStatus.playerState == PlayerSkill.PlaneMode)//verificacao para quando o player retorna ao chao, depois de planar
        {
            AudioManager.instance.Play("Transform");
            _trasformationParticles.Play();//liga particulas de transformacao
            _rb.gravityScale = _playerGravity;//corrige a gravidade quando o aviao toca o chao
            _playerStatus.playerState = PlayerSkill.Normal;//corrige a forma do player
            //_sr.color = Color.white;
            _sr.sprite = _playerStatus.normalSprite;
            Vector3 v = _playerStatus.sr.bounds.size;
            BoxCollider2D b = _playerStatus.collider as BoxCollider2D;
            b.size = v;
        }
    }

    private void BallBreak()
    {
        if (_playerStatus.playerState == PlayerSkill.BallMode) //verificacao para quando o player retorna ao chao, depois de planar
        {
            if (_rb.velocity.y <= -ballVelocityThreshold)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -ballVelocityThreshold);
            }
            
            if ( _rb.velocity.y < 0f && IsGrounded())
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            }
        }


    }

    private void ShurikenBreak()
    {
        if (_playerStatus.playerState == PlayerSkill.ShurikenMode)//verificacao para quando o player retorna ao chao, depois de planar
        {
            _rb.velocity = new Vector2(0f, _rb.velocity.y); 
        }
    }
    
    private void BoatBreak()
    { 
        // O barco anda na agua
        if(OnWater())
            return;
        
        if (_playerStatus.playerState == PlayerSkill.BoatMode) //verificacao para quando o player retorna ao chao, depois de planar
        {
             _rb.velocity = new Vector2(0f, _rb.velocity.y); 
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
    
    //verifica uma posição segura para o player 
    private void SaveSafePosition()
    {
        if (!OnWater())
        {
            _playerStatus._lastSafePos = _playerTransform.position;
            float safeDistance = _collider2D.bounds.size.x * 0.25f;
            _playerStatus._lastSafePos.x += isFlipped ? safeDistance : -safeDistance;
        }
            
    }

    // Detectando colisão com o chão 
    private bool IsGrounded()
    {
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - _collider2D.size.y * 0.25f);
        if(_playerStatus.playerState == PlayerSkill.Normal)
            hitPosition = new Vector2(transform.position.x, transform.position.y - _collider2D.size.y * 0.5f);
        
        LayerMask layers = groundMask;

        if (_playerStatus.playerState == PlayerSkill.BoatMode)
        {
            layers = LayerMask.GetMask("BoatFloor", "Floor");
        }
        Collider2D[] hitGround = Physics2D.OverlapCircleAll(hitPosition, detectGroundRange, layers);
        
      
        // Quebra chao caso seja a bolinha
        if (_playerStatus.playerState == PlayerSkill.BallMode)
        {
            foreach (Collider2D colider in hitGround)
            {
                if (colider.CompareTag("BreakableFloor"))
                {
                    if (_rb.velocity.y <= -10f)
                        Destroy(colider.gameObject); // Trocar por animacao de Chao destruido 
                }
            }
        }
        
        // breake floors with ballmode 

        if (hitGround.Length > 0)
        {
            return true; 
        }
        
        return false;
    }


    private bool OnWater()
    {
        
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - _collider2D.size.y * 0.25f);
        LayerMask waterLayer = LayerMask.GetMask("BoatFloor");
        Collider2D[] hitGround = Physics2D.OverlapCircleAll(hitPosition, detectGroundRange, waterLayer);

        if (hitGround.Length > 0)
        {
            return true; 
        }
        
        return false;
    }
    
    // Checar se ta na  parede para a habilidade da shuriken 
    
    private bool CheckWall()
    {
        Vector3 position = transform.position;
        Vector2 hitPosition = new Vector2(position.x + _collider2D.size.x * 0.5f, position.y);
        if (isFlipped)
            hitPosition = new Vector2(position.x - _collider2D.size.x * 0.5f, position.y);
        
        LayerMask layer = LayerMask.GetMask( "Floor");
        Collider2D[] hitWall= Physics2D.OverlapCircleAll(hitPosition, detectGroundRange, layer);
        
        // breake floors with ballmode 
        if (hitWall.Length > 0)
        {
            return true; 
        }
        
        return false;
        
    }

    private void OnDrawGizmosSelected()
    {

        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y/4);

        Gizmos.DrawWireSphere(hitPosition, detectGroundRange);
    }


}
