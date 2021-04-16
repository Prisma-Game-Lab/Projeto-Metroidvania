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
            AudioManager.instance.Stop(destination.SceneToGo);
            destination.SceneToGo = sceneToLoad;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

public enum TyperOfGate{
    Left,
    Right
}
