using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour
{

    public GameObject UIMaster;
    private GameObject _mailBoxObject;
    [HideInInspector]public bool onDoor = false;
    [HideInInspector] public bool onMailBox = false;
    private PlayerVictory _playerVictory;
    private MailBox _mailBox;

    void Start()
    {
        _playerVictory = GetComponent<PlayerVictory>();
        
    }

    public void Interaction(InputAction.CallbackContext ctx)
    {
        if (onDoor)
        {
            if (_playerVictory.CollectedAll())
            {
                UIMaster.GetComponent<UIMaster>().PlayerWon(true);
            }
            else
            {
                UIMaster.GetComponent<UIMaster>().PlayerWon(false);
            }
        }
        else if (onMailBox)
        {
            _mailBox.mailBoxUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_mailBox.mailBoxUIFirstButton);
            Time.timeScale = 0f;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MailBox"))
        {
            onMailBox = true;
            _mailBoxObject = collision.gameObject;
            _mailBox = _mailBoxObject.GetComponent<MailBox>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onMailBox = false;
    }

    public bool IsInteracting()
    {
        return onMailBox || onDoor;
    }


}
