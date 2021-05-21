using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControls : MonoBehaviour
{
    public List<GameObject> controls;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoRight()
    {
        foreach(GameObject control in controls)
        {
            if (control.activeSelf)
            {
                control.SetActive(false);
                controls.IndexOf(control);
                if (controls.IndexOf(control) == 3)
                {
                    controls[0].SetActive(true);
                    break;
                }
                else
                {
                    controls[controls.IndexOf(control) + 1].SetActive(true);
                    break;
                }
                
            }
        }
    }

    public void GoLeft()
    {
        foreach (GameObject control in controls)
        {
            if (control.activeSelf)
            {
                control.SetActive(false);
                controls.IndexOf(control);
                if (controls.IndexOf(control) == 0)
                {
                    controls[3].SetActive(true);
                    break;
                }
                else
                {
                    controls[controls.IndexOf(control) - 1].SetActive(true);
                    break;
                }

            }
        }
    }
}
