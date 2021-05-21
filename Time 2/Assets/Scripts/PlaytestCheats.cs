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

    public void GetAllStampsAndSkills(InputAction.CallbackContext ctx)
    {
        _playerStatus.SetStampStatus(stampDestination.magenta);
        _playerStatus.SetStampStatus(stampDestination.cyan);
        _playerStatus.SetStampStatus(stampDestination.yellow);
        _playerStatus.SetStampStatus(stampDestination.black);
        _playerStatus.SetStampStatus(stampDestination.lobby);

        foreach (Stamp stamp in mailBox.GetComponent<MailBox>().stamps)
        {
            _playerStatus.UpdateStampStatus(stamp);
        }

        player.GetComponent<BoatSkill>().obtained = true;
        _playerStatus.boat = true;
        player.GetComponent<PlaneSkill>().obtained = true;
        _playerStatus.airplane = true;
        player.GetComponent<ShurikenSkill>().obtained = true;
        _playerStatus.shuriken = true;
        player.GetComponent<PlayerAttack>().obtained = true;
        _playerStatus.sword = true;
        player.GetComponent<BallSkill>().obtained = true;
        _playerStatus.ball = true;

        SaveSystem.SavePlayer(_playerStatus);


    }
}
