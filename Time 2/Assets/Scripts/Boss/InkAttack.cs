using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InkAttack : MonoBehaviour
{
    public GameObject bossSpit;
    [Header("Raio do alcance do cuspe de tinta:")]
    public float attackRadius;
    [Header("Tempo entre o boss detectar o player e realizar o ataque:")]
    public float attackPreparationTime;
    [Header("Tempo de recuperacao entre um cuspe de tinta e outro:")]
    public float resetAttackTime;
    [Header("Velocidade do cuspe de tinta:")]
    public float inkSpeed;
    [Header("Tilemap do chao:")]
    public Tilemap floor;
    public GameObject inkTileFloor;
    public GameObject inkTileWall;
    public GameObject TonguePosition;

    private Transform _transform;
    private Transform _playerTransform;
    private Animator _animator;
    private Rigidbody2D _rb;
    private bool _performingAttack = false;

    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
    }



    public bool PlayerInRange()
    {
        LayerMask layer = LayerMask.GetMask("Player");
        Collider2D[] hit = Physics2D.OverlapCircleAll(_transform.position, attackRadius, layer);

        if(hit.Length > 0)
        {
            if (!_performingAttack)//se o inimigo n�o estiver atacando ou no tempo de espera entre um ataque e outro
            {
                _playerTransform = hit[0].transform;
                return true;
            }
        }

        return false;
    }

    public void PerformInkAttack()
    {
        if ( PlayerInRange())
        {
            _performingAttack = true;
            float direction = 1f;
            if (_playerTransform.position.x < transform.position.x)
            {
                direction = -1f;
            }
            _rb.velocity = Vector2.zero;
            

            Vector2 MovePos = (_playerTransform.position - _transform.position);
            MovePos = MovePos.normalized;
            // _transform.position = MovePos;
            MovePos.x = MovePos.x * inkSpeed;
            MovePos.y = MovePos.y * inkSpeed;
            // _rb.velocity = MovePos;
            StartCoroutine(PrepareAttack(MovePos));
        }
    }

    private IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(resetAttackTime);
        _performingAttack = false;
    }
    private IEnumerator PrepareAttack(Vector2 MovePos)
    {

        //animacaoo de preparacao do cuspe de tinta
        yield return new WaitForSeconds(attackPreparationTime);
        _animator.SetTrigger("Idle");
        AudioManager.instance.Play("Cuspe");
        // criar a bala 
        GameObject bullet = Instantiate(bossSpit, TonguePosition.transform.position, _transform.rotation);
        bullet.GetComponent<InkSpit>().inkTileFloor = inkTileFloor;
        bullet.GetComponent<InkSpit>().inkTileWall = inkTileWall;
        bullet.GetComponent<InkSpit>().floor = floor;
        bullet.GetComponent<Rigidbody2D>().AddForce(MovePos, ForceMode2D.Impulse);
        StartCoroutine(StopAttack());

    }

    public IEnumerator StartInkAttacksCoroutine(int times)
    {
        for(int i = 0; i< times; i++)
        {
            _animator.SetTrigger("Prepare");
            PerformInkAttack();
            yield return new WaitForSeconds(attackPreparationTime + resetAttackTime + 1f);
        }
    }

    public void StartInkAttacks(int times)
    {
        StartCoroutine(StartInkAttacksCoroutine(times));
    }



}


