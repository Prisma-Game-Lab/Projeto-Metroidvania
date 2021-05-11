using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer ==  LayerMask.GetMask("Enemies"))
            return;
        
        Destroy(gameObject);
    }
    
}

