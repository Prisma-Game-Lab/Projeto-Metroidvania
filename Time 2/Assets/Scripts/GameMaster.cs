using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{

    private bool _paused = false;
    // Texto para auxiliar no funcionamento da skill 
    public static GameMaster instance;
    public Destination playerDestination;
    public TeleportDestination teleportDestination;
    public GameObject SettingsUI;
    public GameObject ControlsUI;
    public GameObject PauseMenuUI;

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
            AudioManager.instance.Stop(teleportDestination.SceneToGo);
            playerDestination.SceneToGo = "Lobby";
            teleportDestination.SceneToGo = "Lobby";
            teleportDestination.targetedMailBox = stampDestination.lobby;
            AudioManager.instance.Play(playerDestination.SceneToGo);
            playerDestination.door = -1;
            SceneManager.LoadScene("Lobby");
        }
           
    }
    
    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (_paused)
        {
            PauseMenuUI.SetActive(false);
            _paused = false;
            Time.timeScale = 1f;
            return;
        }

        PauseMenuUI.SetActive(true);
        _paused = true;
        Time.timeScale = 0f;
    }

    public void BackToGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
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


    public void LoadGame()
    {
        playerDestination.SceneToGo = "Lobby";
        playerDestination.door = -1;
        StartCoroutine(WaitFade());
    }

    public void GoToSettings()
    {
        PauseMenuUI.SetActive(false);
        SettingsUI.SetActive(true);
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
     public void BackToPauseMenu()
    {
        PauseMenuUI.SetActive(true);
        SettingsUI.SetActive(false);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("FirstScene");
    }

    private IEnumerator WaitFade()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Lobby");
    }

}
