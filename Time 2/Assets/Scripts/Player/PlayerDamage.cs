using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float hitboxRange = 1f;
    public LayerMask enemyLayers;

    // MUDAR DE LUGAR SOMENTE PARA TESTE
    public PlayerHealth life;
    public int playerLife = 3;

    // Variaveis do efeito de blink
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;
    public float interval = 0.1f;
    public float duration = 0.4f;


    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _takingDamage;

    private void Awake()
    {
        life.life = playerLife;
    }

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        
    }

    private void Update()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, hitboxRange, enemyLayers);

        if(hitEnemies.Length > 0)
        {
            foreach (Collider2D colider in hitEnemies)
            {
                if (colider.gameObject.GetComponent<EnemyDamage>().CanDamage)
                {
                    if (!_takingDamage)
                    {
                        TakeDamage();
                    }
                }
                    
            }
            
        }

    }


    // Função auxiliar para monstrar a área do ataque
    private void OnDrawGizmosSelected()
    {
        if (transform.position == null)
            return;

        Gizmos.DrawWireSphere(transform.position, hitboxRange);
    }

    public void TakeDamage()
    {
        _takingDamage = true;
        // audio de Dano 
        AudioManager.instance.Play("Dano");
        RemoveLife();
        StartCoroutine(FlashSprite());
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
            yield return null;
        }

        _takingDamage = false;
        _sr.color = maxColor;
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
