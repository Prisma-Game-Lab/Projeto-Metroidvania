using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public enum PlayerSkill
{
    Normal,
    AttackSkill,
    BallMode,
    BoatMode,
    PlaneMode
}

public class PlayerStatus : MonoBehaviour
{

    public PlayerSkill playerState = PlayerSkill.Normal;
    
    // Variaveis para serem salvas 
    // Colors
    [HideInInspector] public bool cyan = false;
    [HideInInspector] public bool yellow = false;
    [HideInInspector] public bool magenta = false;
    [HideInInspector] public bool black = false; 
  
    // Skills 
    [HideInInspector] public bool boat = false;
    [HideInInspector] public bool airplane = false;
    [HideInInspector] public bool sword = false;
    [HideInInspector] public bool ball = false;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        // checar se existe player a ser carregado 
        LoadPlayer();
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Verifica a morte pela agua
        if (collision.collider.CompareTag("Water") && (playerState != PlayerSkill.BoatMode))
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
                sword = true;
                break;
            case PlayerSkill.BoatMode:
                gameObject.GetComponent<BoatSkill>().obtained = true;
                boat = true;
                break;
            case PlayerSkill.Normal:
                break;
            case PlayerSkill.PlaneMode:
                gameObject.GetComponent<PlaneSkill>().obtained = true;
                airplane = true; 
                break;
            case PlayerSkill.BallMode:
                gameObject.GetComponent<BallSkill>().obtained = true;
                ball = true; 
                break;
        }

        // Mostrar a descricao de ajuda asobre a habilidade 
        SaveSystem.SavePlayer(this);
        GameMaster.instance.ShowSkillDescription(description);
    }

    private void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null) return;
        cyan = data.cyan;
        yellow = data.yellow; 
        magenta = data.magenta;
        black = data.black;
        
        // Skills 
        boat = data.boat;
        airplane = data.airplane;
        sword = data.sword;
        ball = data.ball;
        
        // liberar as skills de acordo com o save 
        gameObject.GetComponent<BoatSkill>().obtained = boat;
        gameObject.GetComponent<PlayerAttack>().obtained = sword;
        gameObject.GetComponent<PlaneSkill>().obtained = airplane;
        gameObject.GetComponent<BallSkill>().obtained = ball;

    }

}
