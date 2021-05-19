using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MailBox : MonoBehaviour
{
    // Start is called before the first frame update
    public stampDestination myMailBox;
    public List<Stamp> stamps;//em toda caixa de correio existirao todos os selos, mas eles estarao "desativados"
    public GameObject player;
    public GameObject mailBoxUI;
    [HideInInspector]public PlayerStatus playerStatus;


    private void Start()
    {
        playerStatus = player.GetComponent<PlayerStatus>();
        UpdateStampStatus();
    }
    
    public void LeaveMailBox()
    {
        mailBoxUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdateStampStatus()
    {
        foreach (Stamp stamp in stamps)
        {//verificar quais selos ja foram obtidos pelo jogador ao iniciar a cena

            switch (stamp.mailBoxToGo)
            {
                case stampDestination.magenta:
                    stamp.obtained = playerStatus.stampMagenta;
                    break;
                case stampDestination.cyan:
                    stamp.obtained = playerStatus.stampCyan;
                    break;
                case stampDestination.yellow:
                    stamp.obtained = playerStatus.stampYellow;
                    break;
                case stampDestination.black:
                    stamp.obtained = playerStatus.stampBlack;
                    break;
            }


        }
    }
 
}

