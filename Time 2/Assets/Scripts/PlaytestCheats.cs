using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaytestCheats : MonoBehaviour
{

    public GameObject player;
    public GameObject mailBox;
    private PlayerStatus _playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        _playerStatus = player.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllStamps(InputAction.CallbackContext ctx)
    {
        _playerStatus.SetStampStatus(stampDestination.magenta);
        _playerStatus.SetStampStatus(stampDestination.cyan);
        _playerStatus.SetStampStatus(stampDestination.yellow);
        _playerStatus.SetStampStatus(stampDestination.black);
        mailBox.GetComponent<MailBox>().UpdateStampStatus();
        
    }
}
