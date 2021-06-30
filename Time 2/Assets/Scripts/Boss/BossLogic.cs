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
    [Header("Numero de ataques em cada rodada")]
    public List<int> NumberOfAttackPerLife;
    
    [Header("Numero de linguadas por ataque em cada rodada")]
    public List<int> NumberOfFastTongueAttacks;
    [HideInInspector] public int life;
    [Header("Numero de cuspes por ataque em cada rodada")]
    public List<int> NumberOfInkAttacks;
    public GameObject BossDoor;

    private bool _canTakeDamage;
    [HideInInspector]public Rain _rain;
    [HideInInspector]public InkAttack _inkAttack;
    void Start()
    {
        // Somente para teste 
        tongueAttack = gameObject.GetComponent<TongueAttack>();
        bossMovement = gameObject.GetComponent<BossMovement>();
        life = NumberOfAttackPerLife.Count;
        _rain = gameObject.GetComponent<Rain>();
        _inkAttack = gameObject.GetComponent<InkAttack>();
        NumberOfAttackPerLife.Reverse();
        NumberOfFastTongueAttacks.Reverse();
        NumberOfInkAttacks.Reverse();
        _canTakeDamage = true;
        StartCoroutine(PerformRounds());


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PerformRounds()
    {
        Debug.Log("antes do while");
        yield return new WaitForSeconds(1f);
        while (life > 0)
        {
            var attacks = NumberOfAttackPerLife[life-1];
            
                for (int i = 0; i < attacks; i++)
                {
                    float waitTime = PerformRandomAttack();
                    yield return new WaitForSeconds(waitTime);
                    /*int lickTimes = NumberOfFastTongueAttacks[life-1];
                    int seconds = 1; 
                    tongueAttack.PerformTongueAttack(seconds,lickTimes);
                    //time to perform fast tongue attacks 
                    yield return new WaitForSeconds(lickTimes * ( seconds + tongueAttack.preparationTime + tongueAttack.tongueFastTime) + 1)*/
                    bossMovement.IsStoped = false;
                    while (!bossMovement.IsStoped)
                    {
                        yield return new WaitForEndOfFrame();
                    }
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
                //Destroy(gameObject);
                BossDied();
            } 
        }

    }

    public float PerformRandomAttack()
    {
        float num = UnityEngine.Random.Range(0f,1f);
        if (num>0.666f)
        {
            int lickTimes = NumberOfFastTongueAttacks[life - 1];
            int seconds = 1;
            tongueAttack.PerformTongueAttack(seconds, lickTimes);
            //time to perform fast tongue attacks 
             return lickTimes * (seconds + tongueAttack.preparationTime + tongueAttack.tongueFastTime) + 1;
            //performar lingua
            //esperar tempo de conclusao da lingua
        }
        else if ((num<0.666f && num>0.333f) || life>=3)
        {
            int times = NumberOfInkAttacks[life - 1];
            _inkAttack.StartInkAttacks(times);
            return times * (_inkAttack.attackPreparationTime + _inkAttack.resetAttackTime + 1f);
            //performar cuspe
            //esperar
        }
        else
        {
            _rain.StartRain();
            return _rain.spitTime + _rain.rainTime + _rain.inkFloorTime + 1f;
            //esperar

        }
    }

    public void BossDied()
    {
        BossDoor.SetActive(false);//trocar por animacao da porta sendo destruida
        Destroy(gameObject);//animacao do boss morrendo
    }
}
