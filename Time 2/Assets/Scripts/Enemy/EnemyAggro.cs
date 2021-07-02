using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    public float aggroRadius;
    public float aggroSpeed;
    public float aggroPreparationTime;

    public float resetAggroTime;

    private EnemyMovement _enemyMovement;
    private Transform _transform;
    private Transform _playerTransform;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private EnemyDamage _enemyDamage;
    private Animator _animator;

    private bool _performingAggro = false;
    // Start is called before the first frame update
    void Start()
    {
        _enemyMovement = gameObject.GetComponent<EnemyMovement>();
        _transform = transform;
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _enemyDamage = gameObject.GetComponent<EnemyDamage>();
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerInRange() && _enemyDamage.CanDamage)
        {
            _enemyMovement.enemyState = EnemyState.Aggro;
            PerformAggro();
        }
    }

    private bool PlayerInRange()
    {
        LayerMask layer = LayerMask.GetMask( "Player");
        Collider2D[] hitWall = Physics2D.OverlapCircleAll(_transform.position, aggroRadius, layer);
        
        // Verificar se o player esta no range 
        if (hitWall.Length > 0)
        {
            // Pegar referencia da posição do player, player sempre sera o primeiro e E unico collider
            if (!_performingAggro)
            {
                _playerTransform = hitWall[0].transform;
            }
            if(_playerTransform.position.y < _transform.position.y + _sr.bounds.size.y*0.5f)
                return true; 
        }
        
        return false;
        
    }

    private void PerformAggro()
    {
        if (!_performingAggro && _enemyMovement.enemyState == EnemyState.Aggro)
        {
            _performingAggro = true;
            float direction = 1f;
            if (_playerTransform.position.x < transform.position.x)
            {
                direction = -1f;
            }
            _rb.velocity = Vector2.zero;
            bool isFlipped = _enemyMovement.isFlipped;
             if (!isFlipped && direction > 0)
             {
                 Vector3 newLocalScale = transform.localScale;
                 newLocalScale.x *= -1;
                 _transform.localScale = newLocalScale;
                 _enemyMovement.isFlipped = true;
             }
            
             // movendo para a direta flipado. Vai flipar 
             if (isFlipped && direction < 0)
             {
                 Vector3 newLocalScale = transform.localScale;
                 newLocalScale.x *= -1;
                 _transform.localScale = newLocalScale;
                 _enemyMovement.isFlipped = false;
             }
            
            Vector2 MovePos = new Vector2(
                    direction * aggroSpeed, //MoveTowards on 1 axis
                    0f
                );
            // _transform.position = MovePos;
            
            // _rb.velocity = MovePos;
            StartCoroutine(PrepareAggro(MovePos));
        }

    }
    
    private IEnumerator StopAggro()
    {
        yield return new WaitForSeconds(resetAggroTime);
        _performingAggro = false;
        _enemyMovement.enemyState = EnemyState.Idle;
    }
    
    private IEnumerator PrepareAggro(Vector2 MovePos)
    {
        AudioManager.instance.Play("Tombo");
        _animator.SetBool("Triggered", true);
        yield return new WaitForSeconds(aggroPreparationTime);
        _animator.SetBool("Triggered", false);
        _rb.AddForce(MovePos, ForceMode2D.Impulse);
        StartCoroutine(StopAggro());
        

    }
    
    // O inimigo agressivo morre na agua
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Verifica a morte pela agua
        if (collision.collider.CompareTag("Water"))
        {
            Debug.Log("agua");
            Destroy(gameObject);
        }
    }
    
    
    // Funcao para debugar as hitboxes 
    // private void OnDrawGizmosSelected()
    // {       
    //         Vector3 position = _transform.position;
    //         Vector2 aggroPosition = new Vector2(position.x, position.y);
    //
    //         Gizmos.DrawWireSphere(aggroPosition, aggroRadius);
    // }
}
