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
    public Destination playerDestination;

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
            playerDestination.SceneToGo = "Lobby";
            playerDestination.door = -1;
            SceneManager.LoadScene("Lobby");
        }
           
    }
    
    
    // DELETE SAVE 
    public void DeleteSave()
    {
        SaveSystem.DeleteSave();
    }
    
    
    // Start is called before the first frame update
    public void StartGame()
    {
        playerDestination.SceneToGo = "Lobby";
        playerDestination.door = -1;
        SaveSystem.DeleteSave();
        StartCoroutine(WaitFade());

    }
    
    private IEnumerator WaitFade()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Lobby");
    }

}
