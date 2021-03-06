using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;


public class GameMaster : MonoBehaviour
{

    private bool _paused = false;
    private PlayerStatus _playerStatus;
    
    // Texto para auxiliar no funcionamento da skill 
    public static GameMaster instance;
    public Destination playerDestination;
    public TeleportDestination teleportDestination;
    public GameObject SettingsUI;
    public GameObject ControlsUI;
    public GameObject PauseMenuUI;
    public GameObject firstButtonPauseMenu;
    public GameObject firstButtonSettings;
    public PlayerHealth life;
    public GameObject firstButtonControls;
    public GameObject player;
    [HideInInspector] public bool onOtherMenu = false;
    private bool _uiActionsEnable = true;
    
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

    private void Start()
    {
        if(player != null)
        {
            _playerStatus = player.GetComponent<PlayerStatus>();
            this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.GlobalActions);
            
           
        }
        if(SceneManager.GetActiveScene().name != "FirstScene")
            ToogleUIActions();
    }


    public void OnReset(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !_paused)
        {
            // parar as musicas de outros leveis
            AudioManager.instance.Stop(playerDestination.SceneToGo);
            AudioManager.instance.Stop(teleportDestination.SceneToGo);
            playerDestination.SceneToGo = "Lobby";
            teleportDestination.SceneToGo = "Lobby";
            teleportDestination.targetedMailBox = stampDestination.lobby;
            life.life = life.totalLife;
            AudioManager.instance.Play(playerDestination.SceneToGo);
            playerDestination.door = -1;
            SceneManager.LoadScene("Lobby");
        }
           
    }
    
    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (!onOtherMenu && life.life>0){
                ToogleUIActions();
                if (_paused)
                {
                    if(life.life > 0)
                        player.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.PlayerActions);
                    //this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.GlobalActions);
                    EventSystem.current.SetSelectedGameObject(null);
                    PauseMenuUI.SetActive(false);
                    _paused = false;
                    Time.timeScale = 1f;
                    return;
                }

                PauseMenuUI.SetActive(true);
                if (life.life > 0)
                    player.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.PlayerInUI);
                //Debug.Log(player.GetComponent<PlayerInput>().currentActionMap.name);
                EventSystem.current.SetSelectedGameObject(firstButtonPauseMenu);
                //this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.PlayerInUI);
                _paused = true;
                Time.timeScale = 0f;
            }
        }
        
    }

    public void BackToGame()
    {
        
        ToogleUIActions();
        PauseMenuUI.SetActive(false);
        if (life.life > 0)
            player.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.PlayerActions);
        //this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.GlobalActions);
        _paused = false;
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
        onOtherMenu = true;
        PauseMenuUI.SetActive(false);
        SettingsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonSettings);
    }

    public void GoToControls()
    {
        onOtherMenu = true;
        SettingsUI.SetActive(false);
        ControlsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonControls);
    }

    public void BackToSettings()
    {
        SettingsUI.SetActive(true);
        ControlsUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstButtonSettings);
        _playerStatus.SetControl();
    }
     public void BackToPauseMenu()
    {
        onOtherMenu = false;
        PauseMenuUI.SetActive(true);
        SettingsUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstButtonPauseMenu);
    }
    public void GoToMainMenu()
    {
        AudioManager.instance.Stop(playerDestination.SceneToGo);
        SceneManager.LoadScene("FirstScene");
    }

    public void Restart()//funcao de botao de restart da cena do boss
    {
        SceneManager.LoadScene("Boss 1");
    }

    private IEnumerator WaitFadeCutscene()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.instance.Stop("Lobby");
        SceneManager.LoadScene("Cutscene");
    }
    private IEnumerator WaitFadeLobby()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Lobby");
    }

    public void SetPortuguese()
    {
        PlayerPrefs.SetInt("Language", 1);
    }

    public void SetEnglish()
    {
        PlayerPrefs.SetInt("Language", 2);
    }

    public void ResetGame()
    {
        // parar as musicas de outros leveis
        AudioManager.instance.Stop("Boss");
        AudioManager.instance.Stop(playerDestination.SceneToGo);
        AudioManager.instance.Stop(teleportDestination.SceneToGo);
        playerDestination.SceneToGo = "Lobby";
        teleportDestination.SceneToGo = "Lobby";
        teleportDestination.targetedMailBox = stampDestination.lobby;
        life.life = life.totalLife;
        AudioManager.instance.Play(playerDestination.SceneToGo);
        playerDestination.door = -1;
        SceneManager.LoadScene("Lobby");
    }

    public void ReturnToLobby()
    {
        AudioManager.instance.Stop("Boss");
        // parar as musicas de outros leveis
        AudioManager.instance.Stop(playerDestination.SceneToGo);
        AudioManager.instance.Stop(teleportDestination.SceneToGo);
        playerDestination.SceneToGo = "Lobby";
        teleportDestination.SceneToGo = "Lobby";
        teleportDestination.targetedMailBox = stampDestination.lobby;
        //life.life = life.totalLife;
        AudioManager.instance.Play(playerDestination.SceneToGo);
        playerDestination.door = -1;
        SceneManager.LoadScene("Lobby");
    }
    
    public void ToogleUIActions()
    {
        if(_uiActionsEnable)
            EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset.Disable();
        else 
            EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset.Enable();

        _uiActionsEnable = !_uiActionsEnable;
    }

    public void GoToMenuFromDeath()
    {
        
        ToogleUIActions();
        AudioManager.instance.Stop(playerDestination.SceneToGo);
        SceneManager.LoadScene("FirstScene");
    }
}
