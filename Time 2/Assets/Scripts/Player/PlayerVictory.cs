using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVictory : MonoBehaviour
{
    [HideInInspector]public PlayerInteraction playerInteraction;
    [HideInInspector]public PlayerStatus playerStatus;
    private void Start()
    {
        playerStatus = gameObject.GetComponent<PlayerStatus>();
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
                    playerStatus.magenta= true;
                    break;
                case ObjectColor.Cyan:
                    playerStatus.cyan = true;
                    break;
                case ObjectColor.Yellow:
                    playerStatus.yellow = true;
                    break;
                case ObjectColor.Black:
                    playerStatus.black = true;
                    break;
            }
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
        if (playerStatus.magenta && playerStatus.cyan && playerStatus.yellow && playerStatus.black)
            return true;
        return false;
    }

    

}
