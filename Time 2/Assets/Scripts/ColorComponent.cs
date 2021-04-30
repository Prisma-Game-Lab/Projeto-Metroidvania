using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorComponent : MonoBehaviour
{
    public ColorItem colorSettings;
    public ObjectColor collectibleColor;


    private void Start()
    {
        colorSettings.itemColor = collectibleColor;
    }
}

