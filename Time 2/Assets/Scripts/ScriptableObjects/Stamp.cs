using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum mailBox
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
    public mailBox mailBoxToGo;
    public Sprite sprite;
    public bool obtained;
}