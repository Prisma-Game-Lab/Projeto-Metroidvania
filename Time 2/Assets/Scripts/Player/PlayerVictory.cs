using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVictory : MonoBehaviour
{
    public GameObject UIMaster;
    public PlayerStatus playerStatus;


    private void Start()
    {
        playerStatus = gameObject.GetComponent<PlayerStatus>();
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
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("FinalDoor"))
        {
            if (CollectedAll())
            {
                UIMaster.GetComponent<UIMaster>().PlayerWon();
            }
        }

    }

    public bool CollectedAll()
    {
        if (playerStatus.magenta && playerStatus.cyan && playerStatus.yellow && playerStatus.black )
            return true;
        return false;
    }
}
