using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBoxListener : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public List<MailBox> mailBoxes;
    public TeleportDestination playerDestination;
    void Start()
    {
        if (player.GetComponent<PlayerStatus>().stampTeleport)
        {
            AudioManager.instance.Play(playerDestination.SceneToGo);
            // No inicio da scena procura atribuir o player a porta que ele adentrou 
            foreach (MailBox mailBox in mailBoxes)
            {
                if (playerDestination.targetedMailBox == mailBox.myMailBox)
                {
                    Vector3 spawPosition = mailBox.gameObject.transform.position;
                    player.transform.position = spawPosition;
                    break;
                }
            }
            player.GetComponent<PlayerStatus>().SetTeleportStatus(false);
        }
        
    }
}
