using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    // Texto para auxiliar no funcionamento da skill 
    public GameObject HelpSkillText;

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
        if(ctx.started)
            SceneManager.LoadScene("POC");
    }

    public void ShowSkillDescription(string description)
    {
        HelpSkillText.GetComponent<Text>().text = description;
        HelpSkillText.SetActive(true);
        StartCoroutine(DisableHelperText());
    }
    
    // DELETE SAVE 
    public void DeleteSave()
    {
        SaveSystem.DeleteSave();
    }

    private IEnumerator DisableHelperText()
    {
        yield return new WaitForSeconds(5.0f);
        HelpSkillText.SetActive(false);
    }
}
