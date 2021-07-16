using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public enum ToungueAttackType
{
    fast,
    slow 
}
public class TongueAttack : MonoBehaviour
{
    // Start is called before the first frame update
    // Range 
    public float rangeRadius;
    public float preparationTime;
    [Header("Tempo que o inimigo demora para mandar a linguada que gruda")]
    public float SlowPreparationTime;
    [Header("Velocidade da lingua, medida em frames ")]
    public float tongueSpeed;
    [Header("Tempo que a lingua fica no ataque rapido")]
    public float tongueFastTime;
    
    [Header("Tempo que a lingua fica presa")]
    public float tongueSlowTime;

    [Header("Prefab da lingua do boss")]
    public GameObject TonguePrefab;

    public GameObject TonguePosition;
    
    private Transform _transform;
    private Vector3 _playerPosition;
    private Animator _animator;
    
    private SpriteRenderer _sr;

    private bool _canAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        _transform = TonguePosition.transform;
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
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
            _canAttack = false;
            yield return new WaitForSeconds(seconds);

            StartCoroutine(PrepareAggro(ToungueAttackType.fast));
            
            yield return new WaitForSeconds(tongueFastTime + preparationTime);
        }
    }

    public void TongueSlowAttack()
    {
        StartCoroutine(PerformTongueSlowAttack());
    }

    private IEnumerator PerformTongueSlowAttack()
    {
        yield return new WaitForSeconds(SlowPreparationTime);
        StartCoroutine(PrepareAggro(ToungueAttackType.slow));
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

    private IEnumerator PerformAttack(GameObject tongue, ToungueAttackType attackType)
    {
        // criar a lingua em direcao ao player
        Vector3 dist = _transform.position - _playerPosition;
        Vector3 initial = _transform.position;
        _animator.SetTrigger("Idle");
        if(attackType == ToungueAttackType.fast)
            AudioManager.instance.Play("Lingua");
        else 
            AudioManager.instance.Play("Lingua_Longa");
        for (int i = 0; i < (int)(tongueSpeed + 1); i++)
        {
            
            Strech(tongue, _transform.position, initial,true);
            initial -= dist * 1.0f/ tongueSpeed;
            
            yield return new WaitForEndOfFrame();
        }
        
        //Strech(tongue, _transform.position, _playerPosition,false);
        if(attackType == ToungueAttackType.fast)
            yield return new WaitForSeconds(tongueFastTime);
        else
        {
            tongue.GetComponent<TongueComponent>().boss = gameObject;
            tongue.GetComponent<EnemyDamage>().imortal = false;
            // TONGUE STUCK SOUND EFFECT 
            
            //
            yield return new WaitForSeconds(tongueSlowTime);
        }
        // sumir ou voltar com a lingua;
        Destroy(tongue);
        _canAttack = true; 
    }

    private IEnumerator PrepareAggro(ToungueAttackType attackType)
    {
        // mostrar animacao de preparacao 
        AudioManager.instance.Play("Tombo");
        _animator.SetTrigger("Prepare");
        yield return new WaitForSeconds(preparationTime);
        if (PlayerInRange())
        {
            yield return new WaitForSeconds(preparationTime*0.5f);
            // criar a lingua 
            GameObject tongue = Instantiate(TonguePrefab, _transform.position, Quaternion.identity);
            StartCoroutine(PerformAttack(tongue, attackType));
        }

    }
    
    public void Strech(GameObject _sprite,Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ) { 
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        centerPos.z = 0;
        _sprite.transform.position = centerPos;
        Vector3 direction = _finalPosition - _initialPosition;
        direction.z = 0;
        direction = Vector3.Normalize(direction);
        _sprite.transform.right = direction;
        float w = _sprite.GetComponent<SpriteRenderer>().sprite.rect.width/_sprite.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        if (_mirrorZ) _sprite.transform.right *= -1f;
        Vector3 scale = new Vector3(1,1,1);
        Vector3 initialPos = _initialPosition;
        initialPos.z = 0;
        Vector3 finalPosition = _finalPosition;
        finalPosition.z = 0;
        
        scale.x = Vector3.Distance(initialPos, finalPosition) ;
        _sprite.transform.localScale = scale;
        
        // float w = sprite.

        //
        // _sprite.transform.position = centerPos;

        // Vector3 scale = _sprite.transform.localScale;
        // scale.x = (_initialPosition.x - _finalPosition.x) / w;
        // _sprite.transform.localScale = scale;
        //
        // Vector3 dir = Vector3.Normalize(_finalPosition - _initialPosition);
        // float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // _sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // Vector3 scale = _sprite.transform.localScale;
        // scale.x = Vector3.Distance(_initialPosition, _finalPosition);
        // _sprite.transform.localScale = scale;
        //
        // 
        // Vector3 centerPos = (initialPosition + finalPosition) / 2f;
        // obj.transform.position = centerPos;
        // Vector3 direction = finalPosition - initialPosition;
        // direction = Vector3.Normalize(direction);
        // obj.transform.right = direction;
        // if (mirrorZ) obj.transform.right *= -1f;
        // Vector3 scale = new Vector3(1, 1, 1);
        // scale.x = Vector3.Distance(initialPosition, finalPosition)/ width;
        // obj.transform.localScale = scale;

    }
}
