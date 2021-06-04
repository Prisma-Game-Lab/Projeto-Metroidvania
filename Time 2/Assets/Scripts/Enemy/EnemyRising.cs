using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRising : MonoBehaviour
{
    
    public float aggroRadius;
    public float aggroSpeed;
    public float aggroPreparationTime;
    public GameObject enemyBullet;

    public float resetAggroTime;

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
            return true;
        }
        
        return false;
        
    }

    private void PerformAggro()
    {
        if (!_performingAggro)
        {
            _performingAggro = true;
            _rb.velocity = Vector2.zero;
            StartCoroutine(PrepareAggro());
        }

    }
    
    private IEnumerator StopAggro()
    {
        yield return new WaitForSeconds(resetAggroTime);
        _performingAggro = false;
        _enemyMovement.enemyState = EnemyState.Idle;
    }
    
    private IEnumerator PrepareAggro()
    {
        // perfoma indicativo de ataque 
        AudioManager.instance.Play("Tombo");
        yield return new WaitForSeconds(aggroPreparationTime);
        //performa animacao de pulo orizontal   
        Vector3 bulletPos = new Vector3(_transform.position.x, _transform.position.y + _sr.bounds.size.y*2f, _transform.position.z);
        GameObject bullet = Instantiate(enemyBullet, bulletPos, _transform.rotation);
        bullet.GetComponent<Rigidbody2D>().gravityScale = 1;
        bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.up*aggroSpeed, ForceMode2D.Impulse);
        StartCoroutine(StopAggro());
    }
    
    // //Funcao para debugar as hitboxes 
    //  private void OnDrawGizmosSelected()
    //  {       
    //          Vector3 position = _transform.position;
    //          Vector2 aggroPosition = new Vector2(position.x, position.y);
    //  
    //          Gizmos.DrawWireSphere(aggroPosition, aggroRadius);
    //  }
}
