using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour
{

    public GameObject UIMaster;
    public GameObject GameMaster;
    private GameObject _mailBoxObject;
    [HideInInspector]public bool onDoor = false;
    [HideInInspector] public bool onMailBox = false;
    private PlayerVictory _playerVictory;
    private MailBox _mailBox;
    private UIMaster _uiMaster;

    void Start()
    {
        _playerVictory = GetComponent<PlayerVictory>();
        _uiMaster = UIMaster.GetComponent<UIMaster>();


    }

    public void Interaction(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (onDoor)
            {
                if (_playerVictory.CollectedAll())
                {
                    _uiMaster.InteractionPanel.SetActive(false);
                    _uiMaster.PlayerWon(true);
                }
                else
                {
                    _uiMaster.InteractionPanel.SetActive(false);
                    _uiMaster.PlayerWon(false);
                }
            }
            else if (onMailBox)
            {
                _uiMaster.InteractionPanel.SetActive(false);
                _mailBox.mailBoxUI.SetActive(true);
                EventSystem.current.SetSelectedGameObject(_mailBox.mailBoxUIFirstButton);
                this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("PlayerInUI");
                GameMaster.GetComponent<PlayerInput>().SwitchCurrentActionMap("PlayerInUI");
                //Time.timeScale = 0f;
            }
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FinalDoor"))
        {
            _uiMaster.InteractionPanel.SetActive(true);
        }
        else if(collision.CompareTag("MailBox")){
            _uiMaster.InteractionPanel.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MailBox"))
        {
            onMailBox = true;
            _mailBoxObject = collision.gameObject;
            _mailBox = _mailBoxObject.GetComponent<MailBox>();
            _mailBox.GetStamp(_mailBox.myMailBox);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onMailBox = false;
        _uiMaster.InteractionPanel.SetActive(false);
    }

    public bool IsInteracting()
    {
        return onMailBox || onDoor;
    }


}
