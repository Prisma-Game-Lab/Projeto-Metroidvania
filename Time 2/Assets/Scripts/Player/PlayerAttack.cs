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
    public LayerMask enemyLayers;
    public ParticleSystem attackParticle;

    // Variaveis para o controle de tempo do ataque
    //attackRate: Quantas vezes dentro de 1 segundo 
    public float attackRate = 0.5f;
    private float _nextAttackTime = 0f;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    // Chamado quando aciona a ação de ataque. 
    public void OnPlayerAttack(InputAction.CallbackContext ctx)
    {
        // Fazer a animação de attack
        if (ctx.started && Time.time >= _nextAttackTime) {

            // Detectar se tem inimigos no range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            ParticleSystem particle = Instantiate(attackParticle, attackPoint.position, attackParticle.transform.rotation);

            // Realizar dano nos inimigos
            foreach (Collider2D enemy in hitEnemies)
            {
                // Realizar dano no inimigo
                enemy.GetComponent<EnemyDamage>().TakeDamage(_playerMovement.isFlipped);
            }

            // resetar o nextAttackTime
            _nextAttackTime = Time.time + 1f / attackRate;
        }
        
    }

    // Função auxiliar para monstrar a área do ataque
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange); 
    }

}
