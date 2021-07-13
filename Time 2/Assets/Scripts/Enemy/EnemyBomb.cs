using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject littleMe;
    public GameObject littleMe2;
    public int numberOfLifes;

    private EnemyMovement _enemyMovement;

    private void Start()
    {
        _enemyMovement = gameObject.GetComponent<EnemyMovement>();
    }

    public void Explode()
    {
        if (numberOfLifes != 0)
        {
            Vector3 pos = transform.position;
            float xDiff = gameObject.GetComponent<Collider2D>().bounds.size.x;
            pos.x -= xDiff;
            Vector3 posFisrtSon = pos;
            Vector3 posSecondSon = pos;
            posFisrtSon.x += xDiff;
            posSecondSon.x -= xDiff;

            GameObject firstSon = Instantiate(littleMe, posFisrtSon, transform.rotation);
            GameObject secondSon = Instantiate(littleMe2, posSecondSon, transform.rotation);
        
            // atualizando o movimento dos filhos 
            EnemyMovement firstSonEnemyMovement = firstSon.GetComponent<EnemyMovement>();
            firstSonEnemyMovement.pointStart = _enemyMovement.pointStart;
            firstSonEnemyMovement.pointEnd = _enemyMovement.pointEnd;
            firstSonEnemyMovement.enabled = true;
            firstSonEnemyMovement.enemyState = EnemyState.Idle;
        
            EnemyMovement secondSonEnemyMovement = secondSon.GetComponent<EnemyMovement>();
            secondSonEnemyMovement.pointStart = _enemyMovement.pointStart;
            secondSonEnemyMovement.pointEnd = _enemyMovement.pointEnd;
            secondSonEnemyMovement.enabled = true;
            secondSonEnemyMovement.enemyState = EnemyState.Idle;
            
            //Fazendo com que os filhos possam dar dano
            EnemyDamage firstSonEnemyDamage = firstSon.GetComponent<EnemyDamage>();
            firstSonEnemyDamage.CanDamage = true;
            firstSonEnemyDamage.enemyLife = 1;
            firstSonEnemyDamage.duration = 0.4f;
            
            
            EnemyDamage secondSonEnemyDamage = secondSon.GetComponent<EnemyDamage>();
            secondSonEnemyDamage.CanDamage = true;
            secondSonEnemyDamage.enemyLife = 1;       
            secondSonEnemyDamage.duration = 0.4f;

            
            // ajeitando para os proximos filhos 
            EnemyBomb firstSonEnemyBomb = firstSon.GetComponent<EnemyBomb>();
            firstSonEnemyBomb.numberOfLifes = numberOfLifes - 1;
            
            EnemyBomb secondSonEnemyBomb = secondSon.GetComponent<EnemyBomb>();
            secondSonEnemyBomb.numberOfLifes = numberOfLifes - 1;
            
            // reduzindo o tamanho dos filhos 
            Vector3 parentLocalScale = transform.localScale;
            firstSon.transform.localScale = parentLocalScale  * 0.50f;
            secondSon.transform.localScale = parentLocalScale * 0.50f;
            
            //Matar o pai, no caso tem que ser a animimacao 
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }
}
