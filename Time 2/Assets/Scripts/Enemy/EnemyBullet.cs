using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]public bool isRain = false;
    [HideInInspector] public float bulletTimeLimit = 0f;

    public void Update()
    {
        if (isRain)
        {
            StartCoroutine(DestroyBullet());
        }
    }  

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Water") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("BreakableFloor"))
        {
            Destroy(gameObject);
            // REALIZAR ANIMACAO DE TINTA ESPARRAMANDO NA PAREDE e depois destruir o objeto
        }

        
    }
    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(bulletTimeLimit);
        Destroy(gameObject);

    }
}

