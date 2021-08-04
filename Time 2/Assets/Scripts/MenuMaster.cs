using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuMaster : MonoBehaviour
{
    public GameObject settingsUI;
    public GameObject creditsUI;
    public GameObject mainMenuUI;
    public GameObject controlsUI;
    public GameObject firstControlUI;
    public GameObject loadGameButton;
    public GameObject warningUI;
    public GameObject languageUI;
    public GameObject firstButtonMain;
    public GameObject firstButtonSettings;
    public GameObject firstButtonControls;
    public GameObject firstButtonfirstControlUI;
    public GameObject firstButtonCredits;
    public GameObject firstButtonWarning;
    public GameObject firstButtonLanguageUI;
    private GameMaster _gameMaster;
    private void Start()
    {
        Time.timeScale = 1f;
        AudioManager.instance.Play("Lobby");
        _gameMaster = GetComponent<GameMaster>();
        EventSystem.current.SetSelectedGameObject(firstButtonLanguageUI);
        if (PlayerPrefs.GetInt("FirstGame")==1)
        {
            openMainMenu();
        }
    }
    public void GoToSettings()
    {
        mainMenuUI.SetActive(false);
        settingsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonSettings);
    }

    public void GoToCredits()
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonCredits);
    }

    public void GoToControls()
    {
        settingsUI.SetActive(false);
        controlsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonControls);
    }

    public void BackToSettings()
    {
        settingsUI.SetActive(true);
        controlsUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstButtonSettings);
    }

    public void BackToMainMenu()
    {
        mainMenuUI.SetActive(true);
        creditsUI.SetActive(false);
        settingsUI.SetActive(false);
        controlsUI.SetActive(false);
        warningUI.SetActive(false);
        if (CheckLoad())
        {
            EventSystem.current.SetSelectedGameObject(loadGameButton);
            return;
        }
        EventSystem.current.SetSelectedGameObject(firstButtonMain);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public bool CheckLoad()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            return true;
        }
        return false;
    }
    public void CallWarning()
    {
        if (CheckLoad())
        {
            warningUI.SetActive(true);
            mainMenuUI.SetActive(false);
            EventSystem.current.SetSelectedGameObject(firstButtonWarning);
        }
        else
        {
            _gameMaster.StartGame();
        }
        
    }

    public void openMainMenu()
    {
        mainMenuUI.SetActive(true);
        languageUI.SetActive(false);
        firstControlUI.SetActive(false);
        if (CheckLoad())
        {
            EventSystem.current.SetSelectedGameObject(loadGameButton);
            loadGameButton.SetActive(true);
            return;
        }
        EventSystem.current.SetSelectedGameObject(firstButtonMain);
    }

    public void goToFirstSettings()
    {
        languageUI.SetActive(false);
        firstControlUI.SetActive(true);      
        EventSystem.current.SetSelectedGameObject(firstButtonfirstControlUI);
    }

}
