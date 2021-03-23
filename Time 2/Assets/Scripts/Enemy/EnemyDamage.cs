using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private SpriteRenderer _sr;

    // Variaveis do efeito de blink
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;
    public float interval = 0.1f;
    public float duration = 0.4f;

    public bool CanDamage = true;
    public int enemyLife = 1;

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
    }

    public void TakeDamage()
    {
        // MUDAR SOMENTE PARA TESTE 
        enemyLife -= 1;
        if (enemyLife == 0)
            CanDamage = false;
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

        float currentInterval = 0;
        while (duration > 0)
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
            duration -= Time.deltaTime;
            yield return null;
        }

        _sr.color = colorNow;

        // MUDAR SOMENTE PARA TESTE 
        if(enemyLife == 0)
            Destroy(gameObject);
    }
}
