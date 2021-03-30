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
    
  }
}