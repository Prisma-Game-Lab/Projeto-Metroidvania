using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float speed;

    [Tooltip("Quantos espacos do tamanho do sprite do inimigo a esquerda quer que o inimigo patrulhe")]
    public float pointStart = 2;

    [Tooltip("Quantos espacos do tamanho do sprite do inimigo a direita quer que o inimigo patrulhe")]
    public float pointEnd = 2;

    public SimpleEnemyMovements enemyMovement;
    
    [HideInInspector] public bool isFlipped = false;
    
    [Tooltip("Force that enemy apply to player when give damage")]
    public Vector2 knockbackForce;
    
    //state of the enemy 
    [HideInInspector] public EnemyState enemyState = EnemyState.Idle;
    private bool _going;
    private Vector3 _originalPos;
    public Vector3 _pointToStart;
    public Vector3 _pointToEnd;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sp;
    public virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sp = gameObject.GetComponent<SpriteRenderer>();
        _going = false;
        _originalPos = transform.position;

        if (enemyMovement == SimpleEnemyMovements.Horizontal)
        {
            _pointToStart = new Vector3(_originalPos.x - sp.bounds.size.x*pointStart, transform.position.y, transform.position.z);
            _pointToEnd = new Vector3(_originalPos.x+ sp.bounds.size.x*pointEnd, transform.position.y, transform.position.z);
            
        }
        else if(enemyMovement == SimpleEnemyMovements.Vertical)
        {
            _pointToStart = new Vector3(transform.position.x, _originalPos.y+ sp.bounds.size.y*pointStart, transform.position.z);
            _pointToEnd = new Vector3(transform.position.x, _originalPos.y + sp.bounds.size.y*pointEnd, transform.position.z);
        }

    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        Flip();
        if(enemyMovement == SimpleEnemyMovements.Horizontal && enemyState == EnemyState.Idle)
        {
            HorizontalMovement();
        }else if (enemyMovement == SimpleEnemyMovements.Vertical && enemyState == EnemyState.Idle)
        {
            VerticalMovement();
        }

        CheckWall();
    }
    

    // Realização de movimento horizontalk simples
    private void HorizontalMovement()
    {

        //Vector3 idleDestination = Vector3.Lerp(_pointToStart,_pointToEnd, Mathf.PingPong(Time.time * speed, 1.0f));
        float direction = 0f;
        if (_going)
        {
            direction = Mathf.Sign(_pointToStart.x - transform.position.x);
        }
        else
        {
            direction = Mathf.Sign(_pointToEnd.x - transform.position.x);
        }
        
        if (transform.position.x < _pointToStart.x + Mathf.Abs(_pointToEnd.x - _pointToStart.x)/10)
        {
            _going = false;
        }
        else if (transform.position.x >_pointToEnd.x - Mathf.Abs(_pointToEnd.x - _pointToStart.x)/10)
        {
            _going = true;
        }
        
        Vector2 MovePos = new Vector2(direction  * speed * Time.deltaTime, //MoveTowards on 1 axis
            rb.velocity.y
        );
        //transform.position = MovePos;
        
        rb.velocity = MovePos;
    }
    
    private void VerticalMovement()
    {
        
        Vector3 idleDestination = Vector3.Lerp(_pointToStart, _pointToEnd, Mathf.PingPong(Time.time * speed, 1.0f));
        float direction = Mathf.Sign(idleDestination.y - transform.position.y);
        Vector2 MovePos = new Vector2(
            transform.position.x, //MoveTowards on 1 axis
            transform.position.y + direction
        );
        transform.position = MovePos;
    }
    
    
    private void Flip()
    {
        // movendo para a esquerda não flipado. Vai flipar 
        if (enemyState == EnemyState.Idle)
        {
            if (!isFlipped && rb.velocity.x > 0)
            {
                Vector3 newLocalScale = transform.localScale;
                newLocalScale.x *= -1;
                transform.localScale = newLocalScale;
                isFlipped = true;
            }

            // movendo para a direta flipado. Vai flipar 
            if (isFlipped && rb.velocity.x < 0)
            {
                Vector3 newLocalScale = transform.localScale;
                newLocalScale.x *= -1;
                transform.localScale = newLocalScale;
                isFlipped = false;
            }
        }
    }
    
    private void CheckWall()
    {
        // inimigo ciano se move de forma diferente 

        Vector3 position = transform.position;
        Vector2 hitPosition = new Vector2(position.x - sp.bounds.size.x * 0.25f, position.y);

        if (isFlipped)
            hitPosition = new Vector2(position.x + sp.bounds.size.x * 0.25f, position.y);
        
        LayerMask layer = LayerMask.GetMask( "Floor", "Water");
        Collider2D[] hitWall = Physics2D.OverlapBoxAll(hitPosition, new Vector2(0.1f,0.1f),0f ,layer);
        
        Collider2D[] hitWall2 = {};
        // CIANO ENEMY CHECK 
        if (gameObject.GetComponent<EnemyRising>() != null)
        {
            hitPosition.y += sp.bounds.size.y;
            hitWall2 = Physics2D.OverlapBoxAll(hitPosition, new Vector2(0.1f,0.1f),0f ,layer);
        }
        
        if (hitWall.Length > 0 || hitWall2.Length > 0)
        {
            if (!isFlipped)
            {
                _pointToStart = transform.position;
                _pointToEnd = new Vector3(position.x + pointStart * pointEnd * sp.bounds.size.x, position.y,
                    position.z);
            }
            else
            {
                _pointToEnd = transform.position;
                _pointToStart = new Vector3(position.x - pointStart * pointEnd * sp.bounds.size.x, position.y,
                    position.z);
            }

        }

    }
    
    private void OnDrawGizmosSelected()
    {
        if (sp != null)
        {
            Vector3 position = transform.position;
            float cyanYOffset = 0.0f;
            if (gameObject.GetComponent<EnemyRising>() != null)
                cyanYOffset = sp.bounds.size.y;
            Vector2 hitPosition = new Vector2(position.x - sp.bounds.size.x * 0.5f, position.y + cyanYOffset);
            if (isFlipped)
                hitPosition = new Vector2(position.x + sp.bounds.size.x * 0.5f, position.y + cyanYOffset);
            
            Gizmos.DrawCube(hitPosition, new Vector2(0.1f,0.1f));
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
