using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpListener : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public GameObject player;
    public List<SkillItem> powerUps;
    void Start()
    {
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        // No inicio da scena procura atribuir o player a porta que ele adentrou 
        foreach (SkillItem powerUp in powerUps)
        {
            if (powerUp.skill == PlayerSkill.AttackSkill && playerStatus.sword )
            {
               powerUp.gameObject.SetActive(false);
               continue;
            }
            
            if (powerUp.skill == PlayerSkill.PlaneMode && playerStatus.airplane )
            {
                powerUp.gameObject.SetActive(false);
                continue;
            }
            
            if (powerUp.skill == PlayerSkill.BoatMode && playerStatus.boat )
            {
                powerUp.gameObject.SetActive(false);
                continue;
            }
            
            if (powerUp.skill == PlayerSkill.ShurikenMode && playerStatus.shuriken )
            {
                powerUp.gameObject.SetActive(false);
                continue;
            }
            
            if (powerUp.skill == PlayerSkill.BallMode && playerStatus.ball )
            {
                powerUp.gameObject.SetActive(false);
               
            }
        }
    }
}
