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
    [HideInInspector] public bool onDoor = false;
    [HideInInspector] public bool onMailBox = false;
    // life statue interaction 
    [HideInInspector] public bool onFillHeartSTatue = false;
    [HideInInspector] public bool onAddHeartStatue = false;
    private int _statueId;
    private PlayerVictory _playerVictory;
    private PlayerStatus _playerStatus;
    private MailBox _mailBox;
    private UIMaster _uiMaster;

    void Start()
    {
        _playerVictory = GetComponent<PlayerVictory>();
        _playerStatus = GetComponent<PlayerStatus>();
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
                GameMaster.GetComponent<GameMaster>().onOtherMenu = true;
                EventSystem.current.SetSelectedGameObject(_mailBox.mailBoxUIFirstButton);
                this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.PlayerInUI);
                GameMaster.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.PlayerInUI);
                //Time.timeScale = 0f;
            }
            else if (onAddHeartStatue)
            {
                _playerStatus.playerLifeStatue.AddNewHeart(_statueId);
            }else if (onFillHeartSTatue)
            {
                _playerStatus.playerLifeStatue.FillLife();
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

        if (collision.CompareTag("FillLifeStatue"))
        {
            onFillHeartSTatue = true;
        }
        
        if (collision.CompareTag("NewHeartStatue"))
        {
            _statueId = collision.GetComponent<StatueData>().id;
            onAddHeartStatue = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onMailBox = false;
        _uiMaster.InteractionPanel.SetActive(false);
        onFillHeartSTatue = false;
        onAddHeartStatue = false;
    }

    public bool IsInteracting()
    {
        return onMailBox || onDoor;
    }


}
