using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaytestCheats : MonoBehaviour
{

    public GameObject player;
    public GameObject mailBox;
    private PlayerStatus _playerstatus;

    // Start is called before the first frame update
    void Start()
    {
        _playerstatus = player.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllStampsAndSkills(InputAction.CallbackContext ctx)
    {
        _playerstatus.SetStampStatus(stampDestination.magenta);
        _playerstatus.SetStampStatus(stampDestination.cyan);
        _playerstatus.SetStampStatus(stampDestination.yellow);
        _playerstatus.SetStampStatus(stampDestination.black);
        _playerstatus.SetStampStatus(stampDestination.lobby);

        foreach (Stamp stamp in mailBox.GetComponent<MailBox>().stamps)
        {
            _playerstatus.UpdateStampStatus(stamp);
        }

        player.GetComponent<BoatSkill>().obtained = true;
        _playerstatus.boat = true;
        player.GetComponent<PlaneSkill>().obtained = true;
        _playerstatus.airplane = true;
        player.GetComponent<ShurikenSkill>().obtained = true;
        _playerstatus.shuriken = true;
        player.GetComponent<PlayerAttack>().obtained = true;
        _playerstatus.sword = true;
        player.GetComponent<BallSkill>().obtained = true;
        _playerstatus.ball = true;

        _playerstatus.cyan = true;
        _playerstatus.black = true;
        _playerstatus.magenta = true;
        _playerstatus.yellow = true;

        _playerstatus.playerHealth.totalLife = 9;
        _playerstatus.playerHealth.life = 9;



        SaveSystem.SavePlayer(_playerstatus);

        Debug.Log("Os Cheats estao comentados");

    }
}
