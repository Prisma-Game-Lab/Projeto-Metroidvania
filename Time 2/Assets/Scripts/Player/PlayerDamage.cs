using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    //public float hitboxRange = 1f;
    public LayerMask enemyLayers;

    // MUDAR DE LUGAR SOMENTE PARA TESTE
    public PlayerHealth life;
    public int playerLife = 3;
    // Variaveis do efeito de blink
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;
    public float interval = 0.1f;
    public float duration = 0.4f;

    public ParticleSystem damageParticle;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    public bool takingDamage;
    public bool cantChange;
    private PlayerStatus _playerStatus;

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        //life.totalLife = _playerStatus.totalLife;
        cantChange = false;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 size = new Vector2(_playerStatus.playerCollider.size.x * 1.1f, _playerStatus.playerCollider.size.y * 1.1f);
        if (_playerStatus.playerState == PlayerSkill.BallMode)
            size = new Vector2(_playerStatus.playerCircleCollider.radius * 2.1f,
                _playerStatus.playerCircleCollider.radius * 2.1f);
        
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(transform.position, size,0f ,enemyLayers);

        if(hitEnemies.Length > 0)
        {
            foreach (Collider2D colider in hitEnemies)
            {
                if (colider.gameObject.GetComponent<EnemyDamage>().CanDamage)
                {
                    if (!takingDamage)
                    {
                        colider.gameObject.GetComponent<EnemyMovement>().enemyState = EnemyState.Idle;
                        colider.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        Vector2 knockbackForce = colider.gameObject.GetComponent<EnemyMovement>().knockbackForce;
                        
                        _playerStatus.rb.velocity = new Vector2(0f, _playerStatus.rb.velocity.y);
                        if (colider.transform.position.x > transform.position.x)
                        {
                            _playerStatus.rb.AddForce(new Vector2(-knockbackForce.x,knockbackForce.y), ForceMode2D.Impulse);
                        }
                        else
                        {
                            _playerStatus.rb.AddForce(new Vector2(knockbackForce.x,knockbackForce.y), ForceMode2D.Impulse);
                        }
                        
                        // ball case 
                        if(colider.gameObject.GetComponent<EnemyBullet>() != null)
                        {
                            Destroy(colider.gameObject);
                        }
                        
                        TakeDamage();
                    }
                    else
                    {
                        // ball case 
                        if(colider.gameObject.GetComponent<EnemyBullet>() != null)
                        {
                            Destroy(colider.gameObject);
                        }
                    }
                }
                    
            }
            
        }
    }


    // Função auxiliar para monstrar a área do ataque
    // private void OnDrawGizmosSelected()
    // {
    //     if (transform.position == null)
    //         return;
    //
    //     Gizmos.DrawWireCube(transform.position, new Vector3(_playerStatus.playerCollider.size.x, _playerStatus.playerCollider.size.y,0.1f));
    // }

    public void TakeDamage()
    {
        takingDamage = true;
        cantChange = true;
        // audio de Dano 
        AudioManager.instance.Play("Dano");
        RemoveLife();
        StartCoroutine(FlashSprite());
        damageParticle.Play();
        _playerStatus.playerAnimator.enabled = false;
    }

    public void TakeEnvironmentDamage()
    {
        // audio de Dano 
        AudioManager.instance.Play("Dano");
        RemoveLife();
    }

    ///minAlpha: valor minimo de alpha 
    ///maxAlpha: valor maximo de alpha
    ///interval: intervalo de cada piscada em segundos
    ///duration: tempo do efeito em segundos 
    private IEnumerator FlashSprite()
    {
        Color colorNow = _sr.color;
        Color minColor = new Color(_sr.color.r, _sr.color.g, _sr.color.b, minAlpha);
        Color maxColor = new Color(_sr.color.r, _sr.color.g, _sr.color.b, maxAlpha);
        float ctxDuration = duration;
        float currentInterval = 0;
        while (ctxDuration > 0)
        {
            float tColor = currentInterval / interval;
            _sr.color = Color.Lerp(minColor, maxColor, tColor);

            currentInterval += Time.deltaTime;
            if (currentInterval >= interval)
            {
                Color temp = minColor;
                minColor = maxColor;
                maxColor = temp;
                currentInterval = currentInterval - interval;
            }
            ctxDuration -= Time.deltaTime;
            if (ctxDuration <= duration / 2f)
            {
                takingDamage = false;

            }

            yield return null;
        }

        if (!(_playerStatus.isTight && _playerStatus.playerState == PlayerSkill.BallMode))
        {
            _playerStatus.playerAnimator.enabled = true;
                
            if(_playerStatus.playerState != PlayerSkill.Normal)
                _playerStatus.SetToNormalState();
        }
    
        _sr.color = _sr.color;
        cantChange = false;

    }


    public void KillPlayer()
    {
        Destroy(gameObject);
    }

    // MUDAR IMPLEMENTACAO SOMENTE PARA TESTE
    private void RemoveLife()
    {
        life.life -= 1;
        if(life.life <= 0)
        {
            KillPlayer();
        }
    }
    
    

    

}
