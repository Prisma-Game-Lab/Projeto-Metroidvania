using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // Chamado quando aciona a ação de ataque. 
    public void OnPlayerAttack()
    {
        // Fazer a animação de attack

        // Detectar se tem inimigos no range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Realizar dano nos inimigos
        foreach(Collider2D enemy in hitEnemies)
        {
            // Realizar dano no inimigo
            enemy.GetComponent<EnemyDamage>().TakeDamage();
            Debug.Log("Inimigo foi acertado");
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
