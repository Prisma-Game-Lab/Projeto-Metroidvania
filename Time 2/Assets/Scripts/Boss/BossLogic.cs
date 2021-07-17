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
    [HideInInspector] public Rain rain;
    [HideInInspector] public InkAttack inkAttack;
    [HideInInspector] public bool tongueAttacking = false;
    [HideInInspector] public bool canRain = false;
    [HideInInspector] public bool rained = false;

    private bool _canTakeDamage;
    private Animator _animator;
    private int totalLifes;
    
    void Start()
    {
        // Somente para teste 
        tongueAttack = gameObject.GetComponent<TongueAttack>();
        bossMovement = gameObject.GetComponent<BossMovement>();
        life = NumberOfAttackPerLife.Count;
        totalLifes = life;
        rain = gameObject.GetComponent<Rain>();
        inkAttack = gameObject.GetComponent<InkAttack>();
        NumberOfAttackPerLife.Reverse();
        NumberOfFastTongueAttacks.Reverse();
        NumberOfInkAttacks.Reverse();
        _canTakeDamage = true;
        _animator = gameObject.GetComponent<Animator>();
        _animator.SetBool("Black", true);
        _animator.SetTrigger("FadeOut");
        
    }

    public void StartLogic()
    {
        StartCoroutine(PerformRounds());
    }

    private IEnumerator PerformRounds()
    {
        yield return new WaitForSeconds(1f);
        while (life > 0)
        {
            var attacks = NumberOfAttackPerLife[life-1];
            int beforeLife = life;
            if (life <= totalLifes-1)
                canRain = true;
            
            for (int i = 0; i < attacks; i++)
            {
                if((i-1) == attacks && !rained && canRain)
                {
                    rain.StartRain();
                    yield return new WaitForSeconds(rain.spitTime + rain.rainTime + rain.inkFloorTime + 1f);
                    tongueAttacking = false;
                    rained = true;
                    break;
                }

                float waitTime = PerformRandomAttack();
                yield return new WaitForSeconds(waitTime);

                /*int lickTimes = NumberOfFastTongueAttacks[life-1];
                int seconds = 1; 
                tongueAttack.PerformTongueAttack(seconds,lickTimes);
                //time to perform fast tongue attacks 
                yield return new WaitForSeconds(lickTimes * ( seconds + tongueAttack.preparationTime + tongueAttack.tongueFastTime) + 1)*/
                _animator.SetTrigger("FadeOut");
                yield return new WaitForSeconds(0.6f);
                bossMovement.IsStoped = false;
                while (!bossMovement.IsStoped)
                {
                    yield return new WaitForEndOfFrame();
                    
                }
                _canTakeDamage = true;
                _animator.SetTrigger("FadeIn");
                yield return new WaitForSeconds(0.6f);
                tongueAttacking = false;
            }
            rained = false;
            // wait to go to new position 
            yield return new WaitForSeconds(3);

            _animator.SetTrigger("BigTongue");
            tongueAttack.TongueSlowAttack();
            tongueAttacking = true;
            // time to perform slow tongue 
            yield return new WaitForSeconds(tongueAttack.tongueSlowTime + tongueAttack.SlowPreparationTime + 1);
            tongueAttacking = false;
            /*if (IsDamaged(beforeLife))
            {
                rain.StartSpecialRain();
                yield return new WaitForSeconds(rain.spitTime + rain.rainTime + rain.inkFloorTime + 1f);
                rain.specialRain = false;
            }*/   
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

    /*public bool IsDamaged(int beforeLife)
    {
        if(beforeLife < life)
        {
            rain.specialRain = true;
            return true;
        }
        return false; 
        
    }*/

    public float PerformRandomAttack()
    {
        float num = UnityEngine.Random.Range(0f,1f);
        if (num>0.666f)
        {
            int lickTimes = NumberOfFastTongueAttacks[life - 1];
            int seconds = 1;
            tongueAttack.PerformTongueAttack(seconds, lickTimes);
            //time to perform fast tongue attacks
            tongueAttacking = true;
             return lickTimes * (seconds + tongueAttack.preparationTime + tongueAttack.tongueFastTime + 1);
        }
        else if ((num<0.666f && num>0.333f) || life>=3)
        {
            int times = NumberOfInkAttacks[life - 1];
            inkAttack.StartInkAttacks(times);
            return times * (inkAttack.attackPreparationTime + inkAttack.resetAttackTime + 1f);
        }
        else
        {
            rain.StartRain();
            AudioManager.instance.Play("Chuva");
            rained = true;
            return rain.spitTime + rain.rainTime + rain.inkFloorTime + 1f;
        }
        
    }

    public void BossDied()
    {
        AudioManager.instance.Play("Boss_Final");
        AudioManager.instance.Stop("Boss");
        AudioManager.instance.Play("Boss_Morte");
        BossDoor.SetActive(false);//trocar por animacao da porta sendo destruida
        Destroy(gameObject);//animacao do boss morrendo
    }
}
