using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateListener : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public List<GateComponent> gates;
    public Destination playerDestination;
    void Start()
    {
        if (!player.GetComponent<PlayerStatus>().stampTeleport)
        {
            //AudioManager.instance.Play(playerDestination.SceneToGo);
            AudioManager.instance.Play(SceneManager.GetActiveScene().name);
            // No inicio da scena procura atribuir o player a porta que ele adentrou 
            foreach (GateComponent gate in gates)
            {
                if (playerDestination.door == gate.myDoor)
                {
                    Vector3 spawPosition = gate.gameObject.transform.position;
                    float offset = gate.gameObject.GetComponent<BoxCollider2D>().size.x * 0.5f;
                    spawPosition.x += offset;
                    switch (gate.typerOfGate)
                    {
                        case TyperOfGate.Left:
                            spawPosition.x += 3f;
                            break;
                        case TyperOfGate.Right:
                            spawPosition.x -= 3f;
                            Vector3 newLocalScale = player.transform.localScale;
                            newLocalScale.x *= -1;
                            player.transform.localScale = newLocalScale;
                            player.GetComponent<PlayerMovement>().isFlipped = true;
                            break;
                    }

                    player.transform.position = spawPosition;
                }
            }
        }
        
    }
    
}
