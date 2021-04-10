using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    // Texto para auxiliar no funcionamento da skill 
  
    private UIMaster _UIMaster;
    public static GameMaster instance;

    void Awake()
    { 
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            
            Destroy(this);
        }
    }


    public void OnReset(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
            SceneManager.LoadScene("POC");
    }

    public void ShowSkillDescription(string description)
    {
        _UIMaster.ShowSkillDescription(description);
    }
    
    // DELETE SAVE 
    public void DeleteSave()
    {
        SaveSystem.DeleteSave();
    }

}
