using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour
{
    // skill que o item vai dar para o player
    public PlayerSkill skill;

    // texto explicativo
    public string helpDescription;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStatus>().SetPlayerSkill(skill, helpDescription);
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        VerticalMovement();
    }

    // Realização de movimento vertical simples
    private void VerticalMovement()
    {
        /// fazer a skill ficar pulando 
        transform.position = transform.position + transform.up * Mathf.Sin(3f*Time.time) * 1f/50f;
    }

}
