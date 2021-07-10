using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectColor
{
    None,
    Magenta,
    Cyan,
    Yellow,
    Black
}
[CreateAssetMenu]
public class ColorItem : ScriptableObject {

    public ObjectColor itemColor;
    //public string colorText;
}
