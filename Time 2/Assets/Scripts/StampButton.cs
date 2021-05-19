using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StampButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Stamp stamp;
    public TeleportDestination destination;
    public GameObject player;

    private void OnEnable()
    {
        if (stamp.obtained)
            GetComponent<Button>().interactable = true;

        transform.GetChild(0).GetComponent<Text>().text = stamp.destinationName;
    }

    public void Teleport()
    {
        Time.timeScale = 1f;
        // para a musica da fase 
        AudioManager.instance.Stop(destination.SceneToGo);
        destination.SceneToGo = stamp.SceneToGo;
        destination.targetedMailBox = stamp.mailBoxToGo;
        player.GetComponent<PlayerStatus>().SetTeleportStatus(true);
        Debug.Log(player.GetComponent<PlayerStatus>().stampTeleport);
        StartCoroutine(WaitFade());

    }

    private IEnumerator WaitFade()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(destination.SceneToGo);
    }

}
