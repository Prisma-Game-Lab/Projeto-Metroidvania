using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour
{

    public PlayerSkill skill;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<PlayerStatus>().SetPlayerSkill(skill);
            Destroy(gameObject);
        }

    }


}
