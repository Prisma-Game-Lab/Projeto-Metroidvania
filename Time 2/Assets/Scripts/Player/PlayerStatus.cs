using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public enum PlayerSkill
{
    Normal,
    AttackSkill,
    BoatMode,
    PlaneMode
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

        switch (skill)
        {
            case  PlayerSkill.AttackSkill:
                gameObject.GetComponent<PlayerAttack>().obtained = true;
                break;
            case PlayerSkill.BoatMode:
                gameObject.GetComponent<BoatSkill>().obtained = true;
                break;
            case PlayerSkill.PlaneMode:
                gameObject.GetComponent<PlaneSkill>().obtained = true;
                break;
            default:
                break;
        }

        // Mostrar a descri??o de ajuda asobre a habilidade 
        GameMaster.instance.ShowSkillDescription(description);
    }

}
