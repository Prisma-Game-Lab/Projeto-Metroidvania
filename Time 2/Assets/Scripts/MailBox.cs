using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailBox : MonoBehaviour
{
    // Start is called before the first frame update
    public mailBox myMailBox;
    public List<Stamp> stamps;//em toda caixa de correio existirao todos os selos, mas eles estarao "desativados"
    public GameObject player;
    public GameObject mailBoxUI;
    [HideInInspector]public PlayerStatus playerStatus;


    private void Start()
    {

        playerStatus = player.GetComponent<PlayerStatus>();

        foreach( Stamp stamp in stamps){//verificar quais selos ja foram obtidos pelo jogador ao iniciar a cena

            switch (stamp.mailBoxToGo)
            {
                case mailBox.magenta:
                    stamp.obtained = playerStatus.stampMagenta;
                    break;
                case mailBox.cyan:
                    stamp.obtained = playerStatus.stampCyan;
                    break;
                case mailBox.yellow:
                    stamp.obtained = playerStatus.stampYellow;
                    break;
                case mailBox.black:
                    stamp.obtained = playerStatus.stampBlack;
                    break;
            }    


        }
    }
    
    public void LeaveMailBox()
    {
        mailBoxUI.SetActive(false);
        Time.timeScale = 1f;
    }
 
}

