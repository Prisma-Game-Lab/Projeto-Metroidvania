using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float speed;

    public Transform pointStart;

    public Transform pointEnd;

    public SimpleEnemyMovements enemyMovement;
    
    [HideInInspector] public bool isFlipped = false;
    
    //state of the enemy 
    [HideInInspector] public EnemyState enemyState = EnemyState.Idle;


    // Update is called once per frame
    void Update()
    {
        if(enemyMovement == SimpleEnemyMovements.Horizontal && enemyState == EnemyState.Idle)
        {
            HorizontalMovement();
        }
    }


    // Realização de movimento horizontalk simples
    private void HorizontalMovement()
    {
        Vector3 pointToStart = new Vector3(pointStart.position.x, transform.position.y, transform.position.z);
        Vector3 pointToEnd = new Vector3(pointEnd.position.x, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(pointToStart, pointToEnd, Mathf.PingPong(Time.time * speed, 1.0f));
    }

    private void Flip()
    {
        Vector3 pos = transform.position;
        // movendo para a esquerda não flipado. Vai flipar 
        if (!isFlipped && pos.x == pointEnd.position.x )
        {
            Vector3 newLocalScale = transform.localScale;
            newLocalScale.x *= -1;
            transform.localScale = newLocalScale;
            isFlipped = true;
        }

        // movendo para a direta flipado. Vai flipar 
        if (isFlipped && pos.x == pointStart.position.x )
        {
            Vector3 newLocalScale = transform.localScale;
            newLocalScale.x *= -1;
            transform.localScale = newLocalScale;
            isFlipped = false;
        }
            
    }
}

public enum SimpleEnemyMovements
{
    Horizontal,
    Vertical
}

public enum EnemyState
{
    Idle,
    Damaged
}
