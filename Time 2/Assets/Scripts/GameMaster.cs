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
    public static GameMaster instance;

    void Awake()
    { 
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    public void OnReset(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            SceneManager.LoadScene("Lobby");
        }
           
    }
    
    
    // DELETE SAVE 
    public void DeleteSave()
    {
        SaveSystem.DeleteSave();
    }

}
