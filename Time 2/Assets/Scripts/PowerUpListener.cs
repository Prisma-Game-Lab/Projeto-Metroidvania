using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpListener : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public GameObject player;
    public List<SkillStatue> powerUps;
    public List<StatueData> heartStatues;
    void Start()
    {
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        // No inicio da scena procura atribuir o player a porta que ele adentrou 
        foreach (SkillStatue powerUp in powerUps)
        {
            if (powerUp.skill == PlayerSkill.AttackSkill && playerStatus.sword )
            {
               powerUp.idleParticle.Stop();
               continue;
            }
            
            if (powerUp.skill == PlayerSkill.PlaneMode && playerStatus.airplane )
            {
                powerUp.idleParticle.Stop();
                continue;
            }
            
            if (powerUp.skill == PlayerSkill.BoatMode && playerStatus.boat )
            {
                powerUp.idleParticle.Stop();
                continue;
            }
            
            if (powerUp.skill == PlayerSkill.ShurikenMode && playerStatus.shuriken )
            {
                powerUp.idleParticle.Stop();
                continue;
            }
            
            if (powerUp.skill == PlayerSkill.BallMode && playerStatus.ball )
            {
                powerUp.idleParticle.Stop();
               
            }
        }

        foreach (StatueData addHeartStatue in heartStatues)
        {
            for (int i = 0; i < playerStatus.NewHeartsId.Length; i++)
            {
                if (addHeartStatue.id == playerStatus.NewHeartsId[i])
                {
                    addHeartStatue.idleParticle.Stop();
                }
            }
        }
    }
}
