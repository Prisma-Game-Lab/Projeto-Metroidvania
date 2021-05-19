using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum stampDestination
{
    magenta,
    cyan,
    yellow,
    black,
    lobby
}
[CreateAssetMenu]
public class Stamp : ScriptableObject
{
    public string SceneToGo;
    public stampDestination mailBoxToGo;
    public Sprite sprite;
    public bool obtained;
    public string destinationName;
}