using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControls : MonoBehaviour
{
    public List<GameObject> controls;
    private static string _firstGame = "FirstGame";
    private static string _controlPrefs = "ControlPrefs";
    private int _firstGameValue;

    // Start is called before the first frame update
    void Start()
    {
        _firstGameValue = PlayerPrefs.GetInt(_firstGame);
        if(_firstGameValue == 0)
        {
            PlayerPrefs.SetInt(_controlPrefs, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetControlPrefs()
    {
        foreach(GameObject control in controls)
        {
            if (control.activeSelf)
            {
                int option  = controls.IndexOf(control);
                PlayerPrefs.SetInt(_controlPrefs, option);
                Debug.Log(PlayerPrefs.GetInt(_controlPrefs));
            }
        }
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
