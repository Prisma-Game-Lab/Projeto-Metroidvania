using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{

    public GameObject UIMaster;
    [HideInInspector]public bool onDoor = false;
    private PlayerVictory _playerVictory;

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
    }


    

}
