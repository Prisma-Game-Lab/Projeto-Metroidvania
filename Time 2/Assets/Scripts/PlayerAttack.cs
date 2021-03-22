using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public ParticleSystem attackParticle;

    // Chamado quando aciona a ação de ataque. 
    public void OnPlayerAttack(InputAction.CallbackContext ctx)
    {
        // Fazer a animação de attack
        if (ctx.started) {
            // Detectar se tem inimigos no range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            ParticleSystem particle = Instantiate(attackParticle, attackPoint.position, attackParticle.transform.rotation);
            // Realizar dano nos inimigos
            foreach (Collider2D enemy in hitEnemies)
            {
                // Realizar dano no inimigo
                enemy.GetComponent<EnemyDamage>().TakeDamage();
            }

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
