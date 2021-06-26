using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // attackPoint; Transforme de onde ocorre o ataque
    // attackRange: raio de efeito do ataque
    // enemyLayers: Layer dos inimigos
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public ParticleSystem attackParticle;
    
    //SOMENTE PARA DEBUG
    public GameObject dummyAttack;
    [Tooltip("Tempo em segundo que de fato o ataque esta funcionando como dano")]
    public float attackTime; 
    // variaves para ver se já possui o ataque
    public bool obtained = false;

    // Variaveis para o controle de tempo do ataque
    //attackRate: Quantas vezes dentro de 1 segundo 
    public float attackRate = 0.5f;
    private float _nextAttackTime = 0f;
    private PlayerMovement _playerMovement;
    
    private PlayerStatus _playerStatus;
    private PlayerInteraction _playerInteraction;
    private void Start()
    {
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _playerInteraction = gameObject.GetComponent<PlayerInteraction>();
    }

    // Chamado quando aciona a ação de ataque. 
    public void OnPlayerAttack(InputAction.CallbackContext ctx)
    {

        if (_playerInteraction.IsInteracting())
            return;

        // Fazer a animação de attack
        if (_playerStatus.playerState == PlayerSkill.Normal)
        {
            if (ctx.started && Time.time >= _nextAttackTime && obtained) {
                AudioManager.instance.Play("Ataque");
                _playerStatus.playerAnimator.SetTrigger("Attack");

                // resetar o nextAttackTime
                _nextAttackTime = Time.time + 1f / attackRate;
                
                // make the attack 
                StartCoroutine(Attack());
            }
        }

        
    }

    private IEnumerator Attack()
    {
        float ctxDuration = attackTime;
        float enemyDeathDuration = 0.2f;
        List<Collider2D> enemies = new List<Collider2D>();
        while (ctxDuration > 0)
        {
            ctxDuration -= Time.deltaTime;
            LayerMask layers = LayerMask.GetMask("Enemies", "Wall");
            // Detectar se tem inimigos no range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layers);
            // ParticleSystem particle = Instantiate(attackParticle, attackPoint.position, attackParticle.transform.rotation);
            //StartCoroutine(ShowAttack());
            // Realizar dano nos inimigos
            foreach (Collider2D enemy in hitEnemies)
            {
                if(enemies.Contains(enemy))
                    continue;

                if (enemy.CompareTag("Tongue") && !enemy.GetComponent<EnemyDamage>().imortal )
                {
                    enemy.GetComponent<TongueComponent>().boss.GetComponent<BossLogic>().TakeDamage();
                }
                
                if (enemy.CompareTag("Wall")) // Realizar dano no inimigo
                    enemy.GetComponent<EnemyDamage>().TakeStaticDamage();
                else
                {
                    if (enemy.GetComponent<EnemyDamage>() != null)
                    {
                        
                        enemy.GetComponent<EnemyDamage>().TakeDamage(_playerMovement.isFlipped);
                        enemyDeathDuration += enemy.GetComponent<EnemyDamage>().duration;
                    }
                }
                
                
                enemies.Add(enemy);
                      
            }
            yield return new WaitForEndOfFrame();
            
        }

        yield return new WaitForSeconds(enemyDeathDuration);

    }
    // Função auxiliar para monstrar a área do ataque
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange); 
    }

}
