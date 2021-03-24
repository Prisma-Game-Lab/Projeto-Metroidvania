using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float hitboxRange = 1f;
    public LayerMask enemyLayers;

    // MUDAR DE LUGAR SOMENTE PARA TESTE
    public GameObject LifeHud;
    public GameObject GameOverText;
    private int actualLife = 3;

    // Variaveis do efeito de blink
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;
    public float interval = 0.1f;
    public float duration = 0.4f;


    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _takingDamage;

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, hitboxRange, enemyLayers);

        //colidiu com pelo menos 1 inimigo
        if(hitEnemies.Length > 0)
        {
            if (!_takingDamage)
            {
                TakeDamage();
            }
            
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Water")// acrescentar e não tiver a habilidade de barco depois
            KillPlayer();

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
        LifeHud.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        GameOverText.SetActive(true);
        Destroy(gameObject);
    }

    // MUDAR IMPLEMENTACAO SOMENTE PARA TESTE
    private void RemoveLife()
    {
        actualLife -= 1;
        if(actualLife <= 0)
        {
            KillPlayer();
        }
        else
        {
            LifeHud.gameObject.transform.GetChild(actualLife).gameObject.SetActive(false);
        }
      

    }

    

}
