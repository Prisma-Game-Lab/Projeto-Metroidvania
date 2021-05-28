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
        Debug.Log("Stamp Teleport"+player.GetComponent<PlayerStatus>().stampTeleport);
        if (player.GetComponent<PlayerStatus>().stampTeleport)
        {
            AudioManager.instance.Play(playerDestination.SceneToGo);
            foreach (MailBox mailBox in mailBoxes)
            {
                if (playerDestination.targetedMailBox == mailBox.myMailBox)
                {
                    Vector3 spawPosition = mailBox.gameObject.transform.position;
                    player.transform.position = spawPosition;
                    
                    break;
                }
            }
            StartCoroutine(WaitTeleport());
            
        }
        
    }

    private IEnumerator WaitTeleport()
    {
        yield return new WaitForSeconds(1f);
        player.GetComponent<PlayerStatus>().SetTeleportStatus(false);
        Debug.Log(player.GetComponent<PlayerStatus>().stampTeleport);
    }
}
