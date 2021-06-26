using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle,
    Moving,
    Tongue,
    Rain,
    EnemyGen
}
public class BossLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public TongueAttack tongueAttack;
    [HideInInspector] public BossMovement bossMovement;
    
    public int life;
    private bool _canTakeDamage;
    void Start()
    {
        // Somente para teste 
        tongueAttack = gameObject.GetComponent<TongueAttack>();
        bossMovement = gameObject.GetComponent<BossMovement>();
        StartCoroutine(PerformRounds());
        
        _canTakeDamage = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PerformRounds()
    {
        while (life > 0)
        {
            var attacks = 2;
            for (int i = 0; i < attacks; i++)
            {
                int lickTimes = 3;
                int seconds = 2; 
                tongueAttack.PerformTongueAttack(seconds,lickTimes);
                //time to perform fast tongue attacks 
                yield return new WaitForSeconds(lickTimes * ( seconds + tongueAttack.preparationTime + tongueAttack.tongueFastTime) + 1);
                bossMovement.IsStoped = false;
                _canTakeDamage = true;
            }
            // wait to go to new position 
            yield return new WaitForSeconds(3);
            
            tongueAttack.TongueSlowAttack();
            // time to perform slow tongue 
            yield return new WaitForSeconds(tongueAttack.tongueSlowTime + tongueAttack.SlowPreparationTime + 1);
           
        }
    }

    public void TakeDamage()
    {
        // animate receiving damage
        if (_canTakeDamage)
        {
            _canTakeDamage = false;
            life -= 1;
            if (life <= 0)
            {
                Destroy(gameObject);
            } 
        }

    }
}
