using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            AudioManager.instance.Play(playerDestination.SceneToGo);
            // No inicio da scena procura atribuir o player a porta que ele adentrou 
            foreach (GateComponent gate in gates)
            {
                if (playerDestination.door == gate.myDoor)
                {
                    Vector3 spawPosition = gate.gameObject.transform.position;
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
