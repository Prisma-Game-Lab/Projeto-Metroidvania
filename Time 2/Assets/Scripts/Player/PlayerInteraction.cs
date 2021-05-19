using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{

    public GameObject UIMaster;
    public GameObject mailBox;
    [HideInInspector]public bool onDoor = false;
    [HideInInspector] public bool onMailBox = false;
    private PlayerVictory _playerVictory;
    private MailBox _mailBox;

    void Start()
    {
        _playerVictory = GetComponent<PlayerVictory>();
        _mailBox = mailBox.GetComponent<MailBox>();
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
            Time.timeScale = 0f;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MailBox"))
        {
            onMailBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onMailBox = false;
    }




}
