using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class TongueAttack : MonoBehaviour
{
    // Start is called before the first frame update
    // Range 
    
    public float rangeRadius;
    public float preparationTime;
    [Header("Velocidade da lingua, medida em frames ")]
    public float tongueSpeed;
    [Header("Tempo que a lingua fica no ataque rapido")]
    public float tongueFastTime;
    
    [Header("Tempo que a lingua fica presa")]
    public float tongueSlowTime;

    [Header("Prefab da lingua do boss")]
    public GameObject TonguePrefab;
    
    private Transform _transform;
    private Vector3 _playerPosition;
    
    private SpriteRenderer _sr;

    private bool _canAttack = false;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // seconds time for make the attack, times e o numero de ataques 
    public void PerformTongueAttack(float seconds, int times)
    {
        _canAttack = true;
        StartCoroutine(SearchPlayer(seconds, times));
    }

    private IEnumerator SearchPlayer(float seconds, int times)
    {
        for (int i = 0; i < times; i++)
        {
            yield return new WaitUntil(NextAttack);
            _canAttack = false;
            yield return new WaitForSeconds(seconds);
            if (PlayerInRange())
            {
                // atacar
                StartCoroutine(PrepareAggro());
                Debug.Log("Achou o player");
            }
            else
            {
                // animacao de nao achar o playere comecar a procurar de novo 
            }
        }
    }

    private bool NextAttack()
    {
        return _canAttack;
    }

    private bool PlayerInRange()
    {
        LayerMask layer = LayerMask.GetMask( "Player");
        Collider2D[] hitWall = Physics2D.OverlapCircleAll(_transform.position, rangeRadius, layer);
        
        // Verificar se o player esta no range 
        if (hitWall.Length > 0)
        {
            _playerPosition = hitWall[0].transform.position;
            Vector2 origin = new Vector2(_transform.position.x, _transform.position.y);
            Vector3 direction = transform.position - _playerPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y);
            LayerMask groundLayer = LayerMask.GetMask("Floor");
            RaycastHit2D hitGround =  Physics2D.Raycast(origin, -direction2D,groundLayer);
            Debug.DrawRay(origin,-direction2D,Color.red);
            if(hitGround.collider.gameObject.CompareTag("Floor")){
                    _playerPosition = hitGround.point;
            }

            
            return true;
        }
        
        return false;
        
    }

    private IEnumerator PerformAttack(GameObject tongue)
    {
        // criar a lingua em direcao ao player
        Vector3 dist = _transform.position - _playerPosition;
        Vector3 initial = _transform.position;
        for (int i = 0; i < (int)(tongueSpeed + 1); i++)
        {
            
            Strech(tongue, _transform.position, initial,true);
            initial -= dist * 1.0f/ tongueSpeed;
            
            yield return new WaitForEndOfFrame();
        }
        
        //Strech(tongue, _transform.position, _playerPosition,false);
        
        yield return new WaitForSeconds(tongueFastTime);
        // sumir ou voltar com a lingua;
        Destroy(tongue);
        _canAttack = true; 
    }

    private IEnumerator PrepareAggro()
    {
        // mostrar animacao de preparacao 
        AudioManager.instance.Play("Tombo");
        yield return new WaitForSeconds(preparationTime);
        // criar a lingua 
        GameObject tongue = Instantiate(TonguePrefab, _transform.position, Quaternion.identity);
        StartCoroutine(PerformAttack(tongue));
        
    }
    
    public void Strech(GameObject _sprite,Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ) {
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        _sprite.transform.position = centerPos;
        Vector3 direction = _finalPosition - _initialPosition;
        direction = Vector3.Normalize(direction);
        _sprite.transform.right = direction;
        if (_mirrorZ) _sprite.transform.right *= -1f;
        Vector3 scale = new Vector3(1,1,1);
        scale.x = Vector3.Distance(_initialPosition, _finalPosition);
        _sprite.transform.localScale = scale;
    }
}
