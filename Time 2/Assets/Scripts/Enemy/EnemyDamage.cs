using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDamage : MonoBehaviour
{
    private SpriteRenderer _sr;
    public UnityEvent dieEvent;

    // Variaveis do efeito de blink
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;
    public float interval = 0.1f;
    public float duration = 0.4f;

    public bool imortal; 

    public bool CanDamage = true;
    
    //forca que lanca o inimigo depois do ataque
    public float damageMagnetude; 
    
    public int enemyLife = 1;

    private Rigidbody2D _rb;

    private bool _takingDamage = false;

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(bool playerFliped)
    {
        // MUDAR SOMENTE PARA TESTE 
        if (imortal)
        {
            return;
        }
        
        enemyLife -= 1;
        if (!_takingDamage)
        {
            _takingDamage = true;
            if (enemyLife == 0 && !imortal)
            {
                gameObject.GetComponent<EnemyMovement>().enabled = false;
                _rb.velocity = Vector2.zero;
                CanDamage = false;
            }

            if (!imortal)
            {
                StartCoroutine(FlashSprite());
            
                // PARA empurrar o inimigo  
                gameObject.GetComponent<EnemyMovement>().enabled = false;
                _rb.velocity = Vector2.zero;
                if(!playerFliped)
                    _rb.AddForce(Vector2.right * damageMagnetude, ForceMode2D.Impulse);
                else
                    _rb.AddForce(Vector2.right * -damageMagnetude, ForceMode2D.Impulse);
            }
        }

        

    }

    public void TakeStaticDamage()
    {
        // MUDAR SOMENTE PARA TESTE 
        enemyLife -= 1;
        if (enemyLife == 0 && !imortal)
        {
            CanDamage = false;
        }
        // MUDAR SOMENTE PARA TESTE 
        if(enemyLife == 0 && !imortal)
            Destroy(gameObject);
    }
    
    // Funcao para ganhar de novo o aggro

    ///minAlpha: valor minimo de alpha 
    ///maxAlpha: valor maximo de alpha
    ///interval: intervalo de cada piscada em segundos
    ///duration: tempo do efeito em segundos 
    private IEnumerator FlashSprite()
    {
        Color colorNow = _sr.color;
        Color minColor = new Color(_sr.color.r, _sr.color.g, _sr.color.b, minAlpha);
        Color maxColor = new Color(_sr.color.r, _sr.color.g, _sr.color.b, maxAlpha);

        float currentInterval = 0;
        float effectDuration = duration;
        while (effectDuration > 0)
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
            effectDuration -= Time.deltaTime;
            yield return null;
        }

        _sr.color = colorNow;

        // MUDAR SOMENTE PARA TESTE 
        _takingDamage = false;
        
        if(enemyLife <= 0)
            dieEvent.Invoke();
        else
            gameObject.GetComponent<EnemyMovement>().enabled = true;
    }

    public void DeathAnimation()
    {
        // ENQUANTO NAO TEM ANIMACAO 
        Destroy(gameObject);
    }
}
