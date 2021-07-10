using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class PlayerData
{
    // Life
    public int totalLife;

    public int[] newHeartsId;
    // Colors 
    public bool cyan;
    public bool yellow;
    public bool magenta;
    public bool black; 
  
    // Skills 
    public bool boat;
    public bool airplane;
    public bool sword;
    public bool ball;
    public bool shuriken;

    //Stamps
    public bool stampMagenta;
    public bool stampCyan;
    public bool stampYellow;
    public bool stampBlack;
    public bool stampLobby;


    //Teleport Status
    public bool stampTeleport;

    //Color Locks
    public bool lockMagenta;
    public bool lockCyan;
    public bool lockYellow;
    public bool lockBlack;

    // Class init 
    public PlayerData(PlayerStatus player)
    {
        // life
        totalLife = player.totalLife;
        newHeartsId = player.NewHeartsId;
        
        // colors 
        cyan = player.cyan;
        yellow = player.yellow;
        magenta = player.magenta;
        black = player.black;
    
        // skills 
        boat = player.boat;
        airplane = player.airplane;
        sword = player.sword;
        ball = player.ball;
        shuriken = player.shuriken;

        //stamps
        stampMagenta = player.stampMagenta;
        stampCyan = player.stampCyan;
        stampYellow = player.stampYellow;
        stampBlack = player.stampBlack;
        stampLobby = player.stampLobby;

        //Teleport Status
        stampTeleport = player.stampTeleport;

        //Color Locks
        lockMagenta = player.lockMagenta;
        lockCyan = player.lockCyan;
        lockYellow = player.lockYellow;
        lockBlack = player.lockBlack;

}
}
