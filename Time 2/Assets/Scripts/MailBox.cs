using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class MailBox : MonoBehaviour
{
    // Start is called before the first frame update
    public stampDestination myMailBox;
    public List<Stamp> stamps;//em toda caixa de correio existirao todos os selos, mas eles estarao "desativados"
    public GameObject player;
    public GameObject mailBoxUI;
    public GameObject mailBoxUIFirstButton;
    public GameObject gameMaster;
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
        gameMaster.GetComponent<GameMaster>().onOtherMenu = false;
        mailBoxUI.SetActive(false);
        player.GetComponent<PlayerInput>().SwitchCurrentActionMap(playerStatus.PlayerActions);
        gameMaster.GetComponent<PlayerInput>().SwitchCurrentActionMap(playerStatus.GlobalActions);
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
    }

    public void GetStamp(stampDestination destination)
    {
        foreach (Stamp stamp in stamps)
        {
            if (stamp.mailBoxToGo == destination)
                playerStatus.SetStampStatus(stamp.mailBoxToGo);
                playerStatus.UpdateStampStatus(stamp);
        }
    }

    

    


    
}

