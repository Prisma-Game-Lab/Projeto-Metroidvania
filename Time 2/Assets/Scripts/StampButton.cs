using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StampButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Stamp stamp;
    
    private void OnEnable()
    {
        if (stamp.obtained)
            GetComponent<Button>().interactable = true;
    }

}
