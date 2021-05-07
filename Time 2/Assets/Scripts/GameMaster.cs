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

    public GameObject SettingsUI;
    public GameObject CreditsUI;
    public GameObject MainMenuUI;
    public GameObject ControlsUI;

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
            // parar as musicas de outros leveis
            AudioManager.instance.Stop(playerDestination.SceneToGo);
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

    public void GoToSettings()
    {
        MainMenuUI.SetActive(false);
        SettingsUI.SetActive(true);
    }

    public void GoToCredits()
    {
        MainMenuUI.SetActive(false);
        CreditsUI.SetActive(true);
    }

    public void GoToControls()
    {
        SettingsUI.SetActive(false);
        ControlsUI.SetActive(true);
    }

    public void BackToSettings()
    {
        SettingsUI.SetActive(true);
        ControlsUI.SetActive(false);
    }

    public void BackToMainMenu()
    {
        MainMenuUI.SetActive(true);
        CreditsUI.SetActive(false);
        SettingsUI.SetActive(false);
        ControlsUI.SetActive(false);
    }
    
    private IEnumerator WaitFade()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Lobby");
    }

}
