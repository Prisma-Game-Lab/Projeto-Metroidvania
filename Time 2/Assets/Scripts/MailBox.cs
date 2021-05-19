using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
        foreach (Stamp stamp in stamps)
        {
            playerStatus.UpdateStampStatus(stamp);
        }
        
    }
    
    public void LeaveMailBox()
    {
        mailBoxUI.SetActive(false);
        Time.timeScale = 1f;
    }

    

    


    
}

