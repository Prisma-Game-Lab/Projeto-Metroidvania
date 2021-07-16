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
    [HideInInspector] public bool _jumpbreak = false;
    private bool _jumped = false;
    private Transform _playerTransform;
    private Vector3 _originalLocalScale;
    private SpriteRenderer _sr;
    private PlayerStatus _playerStatus;
    private BoxCollider2D _collider2D;
    private PlaneSkill _planeSkill;
    private ParticleSystem _trasformationParticles;
    //checar plataforma que se move 
    private bool _onMovingFloor;

    
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _playerGravity = _rb.gravityScale; // gravidade original do player
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _collider2D = _playerStatus.playerCollider;
        _planeSkill = gameObject.GetComponent<PlaneSkill>();
        _playerTransform = transform;
        _originalLocalScale = _playerTransform.localScale;
        _trasformationParticles = _playerStatus.transformationParticles.GetComponent<ParticleSystem>();
        
    }

        // Esse evento é chamado quando o jogador mexe nos inputs de movimento 
    public void OnPlayerMove(InputAction.CallbackContext ctx)
    {
        // nao se move em quanto está dando respawn
        if (_playerStatus.willRespawn)
        {
            _move = Vector2.zero;
            return;
        }
        
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
                    _playerStatus.playerAnimator.SetTrigger("Jump");
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
            
            
            // Aviao e Bola não pulam 
            if(_playerStatus.playerState == PlayerSkill.PlaneMode || _playerStatus.playerState == PlayerSkill.BallMode)
            {
                return;
            }
            
            // _jumped impede pulos adicionais em paredes e o avião não pode pular
            if (IsGrounded() && !_jumped)
            {
                _playerStatus.playerAnimator.SetBool("Jumping", true);
                AudioManager.instance.Play("Jump");
                _jumpHold = true;
                _jumped = true;
                _jumpbreak = false;
                _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
                _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
             
            //Flight();
        }


        if (_jumpHold && ctx.canceled)
        {
           
            // if (_playerStatus.playerState == PlayerSkill.PlaneMode)
            // {
            //     _trasformationParticles.Play();//liga particulas de transformacao
            //     AudioManager.instance.Play("Transform");
            //     _playerStatus.playerState = PlayerSkill.Normal;
            //     
            //     //_sr.color = Color.white;
            //     _rb.gravityScale = _playerGravity;//corrige a gravidade quando o jogador solta o botao de pulaa
            //     _sr.sprite = _playerStatus.normalSprite;
            //     Vector3 v = _playerStatus.sr.bounds.size;
            //     BoxCollider2D b = _playerStatus.playerCollider as BoxCollider2D;
            //     b.size = v;
            // }
            
            if (_rb.velocity.y > 0f)
                _jumpbreak = true; // cancelou o pulo no ar o pulo deve freiar 
            
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
            
            AudioManager.instance.Play("Transform");
            _trasformationParticles.Play();//liga particulas de transformacao
            //_sr.color = Color.yellow;
            _jumpHold = true;
            _rb.gravityScale = flightGravity;
            _sr.sprite = _planeSkill.planeSprite;
            Vector3 v = _playerStatus.sr.bounds.size;
            BoxCollider2D b = _playerStatus.playerCollider as BoxCollider2D;
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
                AudioManager.instance.Play("Fincar");
            }
            else
            {
                _rb.gravityScale = _playerGravity;
            }
        }
        
        // checar plataforma que se move 
        OnMovingFloor();
        
        
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (_playerStatus.playerState == PlayerSkill.ShurikenMode)
        {
            if (CheckWall() && !IsGrounded() && _rb.velocity.y < 0f)
            {
                _rb.gravityScale = 0f;
                _rb.velocity = new Vector2(_rb.velocity.x,0f);
            }
        }
    }

    private void FixedUpdate()
    {
        // animacao 
        if (_move == Vector2.zero)
        {
            _playerStatus.playerAnimationState = PlayerAnimationState.Idle;
            _playerStatus.playerAnimator.SetBool("Moving", false);
        }
        else
        {
            _playerStatus.playerAnimationState = PlayerAnimationState.Movement;
            _playerStatus.playerAnimator.SetBool("Moving", true);
            if (_playerStatus.playerState == PlayerSkill.BallMode)
            {
                if(_move.x > 0)
                    transform.Rotate(Vector3.forward, -10f);
                else if (_move.x < 0)
                    transform.Rotate(Vector3.forward, 10f);
            }
        }
        
        //Realizando o movimento horizontal 
        if (!(_playerStatus.playerState == PlayerSkill.ShurikenMode && CheckWall()) && !gameObject.GetComponent<PlayerDamage>().takingDamage )
        {
            Vector2 m = _move * (speed * Time.fixedDeltaTime);
            if (!(_onMovingFloor && m.x == 0))
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
        VelocityBreak();
        BallBreak();
        if (IsGrounded())
        {
            SaveSafePosition();// Save Safe position for the player 
            if (!_playerStatus.playerDamage.takingDamage)
            {
                FlightBreak();
                BoatBreak();
                ShurikenBreak();
            }
            _playerStatus.isTight = CheckTunnel();
   
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
        //AVIAO ANTIGO
        // if (_playerStatus.playerState == PlayerSkill.PlaneMode)//verificacao para quando o player retorna ao chao, depois de planar
        // {
        //     AudioManager.instance.Play("Transform");
        //     _trasformationParticles.Play();//liga particulas de transformacao
        //     _rb.gravityScale = _playerGravity;//corrige a gravidade quando o aviao toca o chao
        //     _playerStatus.playerState = PlayerSkill.Normal;//corrige a forma do player
        //     //_sr.color = Color.white;
        //     _sr.sprite = _playerStatus.normalSprite;
        //     Vector3 v = _playerStatus.sr.bounds.size;
        //     BoxCollider2D b = _playerStatus.playerCollider as BoxCollider2D;
        //     b.size = v;
        // }
        
        if (_playerStatus.playerState == PlayerSkill.PlaneMode)//verificacao para quando o player retorna ao chao, depois de planar
        {
            _rb.velocity = new Vector2(0f, _rb.velocity.y); 
        }
        
    }

    private void BallBreak()
    {
        if (_playerStatus.playerState == PlayerSkill.BallMode) //verificacao para quando o player retorna ao chao, depois de planar
        {
            if (IsGrounded())
            {
                if (_rb.velocity.y < -2f)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                    AudioManager.instance.Play("Tombo");
                }

            }
        }
        
    }

    private bool CheckTunnel()
    {   
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y + _playerStatus.playerCircleCollider.radius);
        LayerMask waterLayer = LayerMask.GetMask("Floor");
        Collider2D[] hitGround =  Physics2D.OverlapBoxAll(hitPosition, new Vector2(_playerStatus.playerCollider.size.x * 0.8f, 0.3f),0f ,waterLayer);

        if (hitGround.Length > 0)
        {
            return true; 
        }
        
        return false;
    }
    
    // Impedir o jogador de ganhar uma velocidade muito alta 
    private void VelocityBreak()
    {
        if (_rb.velocity.y <= -ballVelocityThreshold)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -ballVelocityThreshold);
        }
        
        if (_rb.velocity.y >= ballVelocityThreshold)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, ballVelocityThreshold);
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
        if (!OnWater() && !OnSpike() && _playerStatus.playerState != PlayerSkill.PlaneMode && !_playerStatus.isTight)
        {
            Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - _playerStatus.playerCollider.size.y * 0.5f);

            LayerMask layers = groundMask;
            
            layers = LayerMask.GetMask( "Floor");
            Collider2D[] hitGround = Physics2D.OverlapBoxAll(hitPosition, new Vector2(0.01f, 0.05f), 0f, layers);
            
            if (hitGround.Length > 0)
            {
                _playerStatus._lastSafePos = _playerTransform.position;
                float safeDistance = _playerStatus.playerCollider.size.x * 0.5f;
                _playerStatus._lastSafePos.x += _playerTransform.localScale.x < 0 ? safeDistance : -safeDistance;
            }
            
        }
            
    }

    // Detectando colisão com o chão 
    private bool IsGrounded()
    {
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - _playerStatus.playerCollider.size.y * 0.5f);

        LayerMask layers = groundMask;
        layers = LayerMask.GetMask("MovingFloor", "Floor");
        if (_playerStatus.playerState == PlayerSkill.BoatMode)
        {
            layers = LayerMask.GetMask("BoatFloor", "Floor");
        }
        
        Collider2D[] hitGround = Physics2D.OverlapBoxAll(hitPosition, new Vector2(_playerStatus.playerCollider.size.x * 0.9f, 0.1f),0f ,layers);

        if (_playerStatus.playerState == PlayerSkill.Normal)
        {
            if (_rb.velocity.y <= 0.01f && _rb.velocity.y >= -0.01f )
            {
                // deteccao do normal precisa ter uma largura maior facilitador de pulo 
                hitGround = Physics2D.OverlapBoxAll(hitPosition, new Vector2(_playerStatus.playerCollider.size.x * 1.5f, 0.1f),0f ,layers);
            }
            else
            {
                hitGround = Physics2D.OverlapBoxAll(hitPosition, new Vector2(_playerStatus.playerCollider.size.x * 0.9f, 0.1f),0f ,layers);
            }
            
            
        } 

        if (_playerStatus.playerState == PlayerSkill.BallMode) // deteccao do chao da bola precisa ter uma altura maior (y = 0.4)
            hitGround = Physics2D.OverlapCircleAll(hitPosition, _playerStatus.playerCollider.size.x * 0.5f, layers);
        
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
            if (_rb.velocity.y <= 0.01f && _rb.velocity.y >= -0.01f )
            {
                _jumped = false;
            }
            _playerStatus.playerAnimator.SetBool("Jumping", false);
            return true; 
            
        }
        
        return false;
    }


    private bool OnWater()
    {
        
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - _collider2D.size.y * 0.5f);
        LayerMask waterLayer = LayerMask.GetMask("BoatFloor");
        Collider2D[] hitGround =  Physics2D.OverlapBoxAll(hitPosition, new Vector2(_playerStatus.playerCollider.size.x * 0.8f, 0.1f),0f ,waterLayer);

        if (hitGround.Length > 0)
        {
            return true; 
        }
        
        return false;
    }
    
    private bool OnSpike()
    {
        
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - _collider2D.size.y * 0.5f);
        LayerMask waterLayer = LayerMask.GetMask("Spike");
        Collider2D[] hitGround =  Physics2D.OverlapBoxAll(hitPosition, new Vector2(_playerStatus.playerCollider.size.x * 0.8f, 0.1f),0f ,waterLayer);

        if (hitGround.Length > 0)
        {
            return true; 
        }
        
        return false;
    }
    
    private void OnMovingFloor()
    {
        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - _collider2D.size.y * 0.5f);
        LayerMask waterLayer = LayerMask.GetMask("MovingFloor");
        Collider2D[] hitGround =  Physics2D.OverlapBoxAll(hitPosition, new Vector2(_playerStatus.playerCollider.size.x * 0.8f, 0.1f),0f ,waterLayer);

        if (hitGround.Length > 0)
        {
            _onMovingFloor = true;
            return;
        }
        
        _onMovingFloor = false;
    }
    
    // Checar se ta na  parede para a habilidade da shuriken 
    
    private bool CheckWall()
    {
        Vector3 position = transform.position;
        Vector2 hitPosition = new Vector2(position.x + _collider2D.size.x * 0.5f, position.y);
        if (isFlipped)
            hitPosition = new Vector2(position.x - _collider2D.size.x * 0.5f, position.y);
        
        LayerMask layer = LayerMask.GetMask( "Floor");
        Collider2D[] hitWall = Physics2D.OverlapBoxAll(hitPosition, new Vector2(0.1f,_playerStatus.playerCollider.size.y * 0.2f),0f ,layer);
        
        // breake floors with ballmode 
        if (hitWall.Length > 0)
        {
            return true; 
        }
        
        return false;
        
    }

    
    // Funcao para debugar as hitboxes 
    // private void OnDrawGizmosSelected()
    // {
    //     if (_collider2D != null)
    //     {
    //         Vector3 position = transform.position;
    //         //Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y - _playerStatus.playerCollider.size.y*0.5f);
    //         Vector2 hitPosition2 = new Vector2(position.x + _playerStatus.playerCollider.size.x * 0.5f, position.y);
    //         Vector2 safePos = new Vector2(position.x, position.y - _playerStatus.playerCollider.size.y * 0.5f);
    //         
    //         // Gizmos.DrawCube(hitPosition, new Vector2(_playerStatus.playerCollider.size.x, 0.1f));
    //         // Gizmos.DrawCube(hitPosition2, new Vector2(0.1f,_playerStatus.playerCollider.size.y * 0.2f));
    //         // Gizmos.DrawCube(hitPosition2, new Vector2(0.1f,_playerStatus.playerCollider.size.y * 0.2f));
    //         Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y + _playerStatus.playerCircleCollider.radius);
    //         //Gizmos.DrawCube(hitPosition, new Vector2( 0.8f, 0.2f));
    //     }
    // }


}
