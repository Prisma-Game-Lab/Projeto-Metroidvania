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
    void Start()
    {
        // somente para teste 
        tongueAttack = gameObject.GetComponent<TongueAttack>();
        tongueAttack.PerformTongueAttack(3,5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
