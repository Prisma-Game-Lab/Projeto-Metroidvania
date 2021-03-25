using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public enum PlayerSkill
{
    Normal,
    BoatMode
}

public class PlayerStatus : MonoBehaviour
{

    public PlayerSkill playerState = PlayerSkill.Normal;

    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica a morte pela agua
        if (collision.collider.tag == "Water" && (playerState != PlayerSkill.BoatMode))
        {
            gameObject.GetComponent<PlayerDamage>().KillPlayer();
        }

    }

    public void SetPlayerSkill(PlayerSkill skill, string description)
    {
        if (skill == PlayerSkill.BoatMode) { 
            gameObject.GetComponent<BoatSkill>().obtained = true;
            GameMaster.instance.ShowSkillDescription(description);
        }
    }

}
