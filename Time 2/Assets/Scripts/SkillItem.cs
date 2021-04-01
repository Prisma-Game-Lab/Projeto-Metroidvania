using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour
{
    // skill que o item vai dar para o player
    public PlayerSkill skill;

    // texto explicativo
    public string helpDescription;

    public float speed;

    public float verticalOffset;

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
        Vector3 posTop = new Vector3(transform.position.x, transform.position.y + verticalOffset/100f, transform.position.z);
        Vector3 posBottom = new Vector3(transform.position.x, transform.position.y - verticalOffset/100f, transform.position.z);
        transform.position = Vector3.Lerp(posTop, posBottom, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}
