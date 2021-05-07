using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMaster : MonoBehaviour
{
    public GameObject SettingsUI;
    public GameObject CreditsUI;
    public GameObject MainMenuUI;
    public GameObject ControlsUI;
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
