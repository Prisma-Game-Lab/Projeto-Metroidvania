using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class PlayerData
{
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


    //Teleport Status
    public bool stampTeleport;


    // Class init 
    public PlayerData(PlayerStatus player)
  {
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

    //Teleport Status
    stampTeleport = player.stampTeleport;

    }
}
