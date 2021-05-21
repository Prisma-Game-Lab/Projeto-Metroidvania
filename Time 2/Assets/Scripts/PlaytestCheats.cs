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
        player.GetComponent<PlaneSkill>().obtained = true;
        player.GetComponent<ShurikenSkill>().obtained = true;
        player.GetComponent<PlayerAttack>().obtained = true;
        player.GetComponent<BallSkill>().obtained = true;

        SaveSystem.SavePlayer(_playerStatus);


    }
}
