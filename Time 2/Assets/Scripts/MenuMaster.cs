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
    public GameObject firstButtonMain;
    public GameObject firstButtonSettings;
    public GameObject firstButtonControls;
    public GameObject firstButtonCredits;
    public GameObject firstButtonWarning;
    private GameMaster _gameMaster;
    private void Start()
    {
        Time.timeScale = 1f;
        _gameMaster = GetComponent<GameMaster>();
        EventSystem.current.SetSelectedGameObject(firstButtonMain);
        if (CheckLoad())
        {
            loadGameButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(loadGameButton);
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
}
