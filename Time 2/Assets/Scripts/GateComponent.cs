using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneToLoad;
    public int myDoor;
    public int doorToGo;
    public TyperOfGate typerOfGate;

    public Destination destination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            destination.door = doorToGo;
            // para a musica da fase 
            AudioManager.instance.Stop("Boss");
            AudioManager.instance.Stop("Boss_Final");
            AudioManager.instance.Stop(destination.SceneToGo);
            float waitSeconds = other.GetComponent<PlayerGateCinematic>().PerformWarpAnimation(typerOfGate == TyperOfGate.Left);
            StartCoroutine(WaitFade(waitSeconds));
        }
    }

    private IEnumerator WaitFade(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        destination.SceneToGo = sceneToLoad;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(destination.SceneToGo);
        
    }
}



public enum TyperOfGate{
    Left,
    Right
}
