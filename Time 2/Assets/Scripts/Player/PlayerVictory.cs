using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVictory : MonoBehaviour
{
    [HideInInspector]public PlayerInteraction playerInteraction;
    [HideInInspector]public PlayerStatus playerStatus;
    public GameObject UIMaster;
    private UIMaster _uiMaster;

    private void Start()
    {
        playerStatus = gameObject.GetComponent<PlayerStatus>();
        _uiMaster = UIMaster.GetComponent<UIMaster>();
        playerInteraction = gameObject.GetComponent<PlayerInteraction>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Color"))
        {
            ObjectColor collectedColor = collision.GetComponent<ColorComponent>().collectibleColor;
            switch (collectedColor)
            {
                case ObjectColor.Magenta:
                    _uiMaster.ColorPanel.transform.GetChild(0).GetComponent<Text>().text = collision.gameObject.transform.GetChild(0).GetComponent<LanguageComponent>().rightText;
                    _uiMaster.ShowColorText();
                    playerStatus.magenta= true;
                    break;
                case ObjectColor.Cyan:
                    _uiMaster.ColorPanel.transform.GetChild(0).GetComponent<Text>().text = collision.gameObject.transform.GetChild(0).GetComponent<LanguageComponent>().rightText;
                    _uiMaster.ShowColorText();
                    playerStatus.cyan = true;
                    break;
                case ObjectColor.Yellow:
                    _uiMaster.ColorPanel.transform.GetChild(0).GetComponent<Text>().text = collision.gameObject.transform.GetChild(0).GetComponent<LanguageComponent>().rightText;
                    _uiMaster.ShowColorText();
                    playerStatus.yellow = true;
                    break;
                case ObjectColor.Black:
                    _uiMaster.ColorPanel.transform.GetChild(0).GetComponent<Text>().text = collision.gameObject.transform.GetChild(0).GetComponent<LanguageComponent>().rightText;
                    _uiMaster.ShowColorText();
                    playerStatus.black = true;
                    break;
            }
            AudioManager.instance.Play("Vitoria");
            SaveSystem.SavePlayer(playerStatus);
            /*Debug.Log(playerStatus.magenta);
            Debug.Log(playerStatus.black);
            Debug.Log(playerStatus.yellow);
            Debug.Log(playerStatus.cyan);*/
            Destroy(collision.gameObject);
        }
        

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("FinalDoor"))
        {
            playerInteraction.onDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInteraction.UIMaster.GetComponent<UIMaster>().InteractionPanel.SetActive(false);
        playerInteraction.onDoor = false;
    }

    public bool CollectedAll()
    {
        if (playerStatus.lockMagenta && playerStatus.lockCyan && playerStatus.lockYellow && playerStatus.lockBlack)
            return true;
        return false;
    }

    public bool CheckIfLastInk()
    {
        int totalInks = 0;
        if (playerStatus.lockMagenta)
            totalInks++;
        if (playerStatus.lockCyan)
            totalInks++;
        if (playerStatus.lockBlack)
            totalInks++;
        if (playerStatus.lockYellow)
            totalInks++;

        if(totalInks == 4)
        {
            return true;
        }
        return false;

    }
    
    

}
