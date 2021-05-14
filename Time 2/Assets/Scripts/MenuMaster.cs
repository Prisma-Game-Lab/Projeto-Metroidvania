using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMaster : MonoBehaviour
{
    public GameObject settingsUI;
    public GameObject creditsUI;
    public GameObject mainMenuUI;
    public GameObject controlsUI;
    public GameObject loadGameButton;
    public GameObject warningUI;
    private GameMaster _gameMaster;
    private void Start()
    {
        Time.timeScale = 1f;
        _gameMaster = GetComponent<GameMaster>();
        if (CheckLoad())
        {
            loadGameButton.SetActive(true);
        }
    }
    public void GoToSettings()
    {
        mainMenuUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void GoToCredits()
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }

    public void GoToControls()
    {
        settingsUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    public void BackToSettings()
    {
        settingsUI.SetActive(true);
        controlsUI.SetActive(false);
    }

    public void BackToMainMenu()
    {
        mainMenuUI.SetActive(true);
        creditsUI.SetActive(false);
        settingsUI.SetActive(false);
        controlsUI.SetActive(false);
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
        }
        else
        {
            _gameMaster.StartGame();
        }
        
    }
}
