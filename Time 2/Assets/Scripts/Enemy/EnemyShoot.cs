using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class EnemyShoot : MonoBehaviour{
    
    public float aggroRadius;
    public float aggroPreparationTime;
    public GameObject enemyBullet;

    public float resetAggroTime;
    public float bulletSpeed;

    private EnemyMovement _enemyMovement;
    private Transform _transform;
    private Transform _playerTransform;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private bool _performingAggro = false;
    // Start is called before the first frame update
    void Start()
    {
        _enemyMovement = gameObject.GetComponent<EnemyMovement>();
        _transform = transform;
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (PlayerInRange())
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
            
            return true; 
        }
        
        return false;
        
    }

    private void PerformAggro()
    {
        if (!_performingAggro)
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

            Vector2 MovePos = (_playerTransform.position - _transform.position);
            MovePos = MovePos.normalized;
            // _transform.position = MovePos;
            MovePos.x = MovePos.x * bulletSpeed;
            MovePos.y = MovePos.y * bulletSpeed;
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
        
        yield return new WaitForSeconds(aggroPreparationTime);
        // criar a bala 
        GameObject bullet = Instantiate(enemyBullet, _transform.position, _transform.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(MovePos, ForceMode2D.Impulse);
        StartCoroutine(StopAggro());
        
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


