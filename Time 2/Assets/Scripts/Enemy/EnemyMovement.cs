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
    private bool going;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        going = false;
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        if(enemyMovement == SimpleEnemyMovements.Horizontal && enemyState == EnemyState.Idle)
        {
            HorizontalMovement();
        }else if (enemyMovement == SimpleEnemyMovements.Vertical && enemyState == EnemyState.Idle)
        {
            VerticalMovement();
        }
    }


    // Realização de movimento horizontalk simples
    private void HorizontalMovement()
    {
        Vector3 pointToStart = new Vector3(pointStart.position.x, transform.position.y, transform.position.z);
        Vector3 pointToEnd = new Vector3(pointEnd.position.x, transform.position.y, transform.position.z);

        //Vector3 idleDestination = Vector3.Lerp(pointToStart, pointToEnd, Mathf.PingPong(Time.time * speed, 1.0f));
        float direction = 0f;
        if (going)
        {
            direction = Mathf.Sign(pointToStart.x - transform.position.x);
        }
        else
        {
            direction = Mathf.Sign(pointToEnd.x - transform.position.x);
        }
        
        if (transform.position.x < pointToStart.x + Mathf.Abs(pointToEnd.x - pointToStart.x)/10)
        {
            going = false;
        }
        else if (transform.position.x > pointToEnd.x - Mathf.Abs(pointToEnd.x - pointToStart.x)/10)
        {
            going = true;
        }
        
        Vector2 MovePos = new Vector2(direction  * speed * Time.deltaTime, //MoveTowards on 1 axis
            0
        );
        //transform.position = MovePos;
        
        _rb.velocity = MovePos;
    }
    
    private void VerticalMovement()
    {
        Vector3 pointToStart = new Vector3(transform.position.x, pointStart.position.y, transform.position.z);
        Vector3 pointToEnd = new Vector3(transform.position.x, pointEnd.position.y, transform.position.z);

        Vector3 idleDestination = Vector3.Lerp(pointToStart, pointToEnd, Mathf.PingPong(Time.time * speed, 1.0f));
        float direction = Mathf.Sign(idleDestination.y - transform.position.y);
        Vector2 MovePos = new Vector2(
            transform.position.x, //MoveTowards on 1 axis
            transform.position.y + direction
        );
        transform.position = MovePos;
    }

    // private void Flip()
    // {
    //     Vector3 pos = transform.position;
    //     // movendo para a esquerda não flipado. Vai flipar 
    //     if (!isFlipped && pos.x == pointEnd.position.x )
    //     {
    //         Vector3 newLocalScale = transform.localScale;
    //         newLocalScale.x *= -1;
    //         transform.localScale = newLocalScale;
    //         isFlipped = true;
    //     }
    //
    //     // movendo para a direta flipado. Vai flipar 
    //     if (isFlipped && pos.x == pointStart.position.x )
    //     {
    //         Vector3 newLocalScale = transform.localScale;
    //         newLocalScale.x *= -1;
    //         transform.localScale = newLocalScale;
    //         isFlipped = false;
    //     }
    //         
    // }
    
    
    private void Flip()
    {
        // movendo para a esquerda não flipado. Vai flipar 
        if (enemyState == EnemyState.Idle)
        {
            if (!isFlipped && _rb.velocity.x > 0)
            {
                Vector3 newLocalScale = transform.localScale;
                newLocalScale.x *= -1;
                transform.localScale = newLocalScale;
                isFlipped = true;
            }

            // movendo para a direta flipado. Vai flipar 
            if (isFlipped && _rb.velocity.x < 0)
            {
                Vector3 newLocalScale = transform.localScale;
                newLocalScale.x *= -1;
                transform.localScale = newLocalScale;
                isFlipped = false;
            }
        }
    }
}

public enum SimpleEnemyMovements
{
    Horizontal,
    Vertical,
    None
}

public enum EnemyState
{
    Idle,
    Damaged,
    Aggro
}
