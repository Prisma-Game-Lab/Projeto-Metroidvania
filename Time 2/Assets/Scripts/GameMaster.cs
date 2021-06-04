using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
    public GameObject firstButtonPauseMenu;
    public GameObject firstButtonSettings;
    public GameObject firstButtonControls;
    public GameObject player;


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
        if (ctx.started)
        {
            if (_paused)
            {
                player.GetComponent<PlayerInput>().SwitchCurrentActionMap("PlayerActions");
                player.GetComponent<PlayerInput>().SwitchCurrentActionMap("GlobalActions");
                EventSystem.current.SetSelectedGameObject(null);
                PauseMenuUI.SetActive(false);
                _paused = false;
                Time.timeScale = 1f;
                return;
            }

            PauseMenuUI.SetActive(true);
            player.GetComponent<PlayerInput>().SwitchCurrentActionMap("PlayerInUI");
            EventSystem.current.SetSelectedGameObject(firstButtonPauseMenu);
            this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("PlayerInUI");
            _paused = true;
            Time.timeScale = 0f;
        }
        
    }

    public void BackToGame()
    {
        PauseMenuUI.SetActive(false);
        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("PlayerActions");
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
        StartCoroutine(WaitFadeCutscene());

    }


    public void LoadGame()
    {
        playerDestination.SceneToGo = "Lobby";
        playerDestination.door = -1;
        StartCoroutine(WaitFadeLobby());
    }

    public void GoToSettings()
    {
        PauseMenuUI.SetActive(false);
        SettingsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonSettings);
    }

    public void GoToControls()
    {
        SettingsUI.SetActive(false);
        ControlsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonControls);
    }

    public void BackToSettings()
    {
        SettingsUI.SetActive(true);
        ControlsUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstButtonSettings);
    }
     public void BackToPauseMenu()
    {
        PauseMenuUI.SetActive(true);
        SettingsUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstButtonPauseMenu);
    }
    public void GoToMainMenu()
    {
        AudioManager.instance.Stop(playerDestination.SceneToGo);
        SceneManager.LoadScene("FirstScene");
    }

    private IEnumerator WaitFadeCutscene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Cutscene");
    }
    private IEnumerator WaitFadeLobby()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Lobby");
    }

}
