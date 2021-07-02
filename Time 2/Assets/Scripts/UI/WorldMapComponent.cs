using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WorldMapComponent : MonoBehaviour
{
    public GameObject Player;
    public GameObject mapImages;
    public Image pin;
    public GameMaster gameMaster;
    //public Destination playerDestination;
    //public TeleportDestination teleportDestination;
    // Start is called before the first frame update

    [HideInInspector] public bool mapActivated;
    void Start()
    {
        mapActivated = false;
        mapImages.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerMap(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (!mapActivated)
            {

                mapActivated = true;
                mapImages.SetActive(true);
                gameMaster.GetComponent<GameMaster>().onOtherMenu = true;//bloqueia o menu de pause quando o mapa estiver ativado


                pin.rectTransform.position = Player.GetComponent<PinPositions>().ReturnActualPosition().position;


            }
            else
            {
                mapImages.SetActive(false);
                mapActivated = false;
                gameMaster.GetComponent<GameMaster>().onOtherMenu = false;
            }

        }

    }
}
