using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MapComponent : MonoBehaviour
{
    // Start is called before the first frame update
    
    // positions on map 
    public RectTransform whitePos;
    public RectTransform cyanPos;
    public RectTransform magentaPos;
    public RectTransform blackPos;
    public RectTransform yellowPos;

    // images from map UI 
    public GameObject mapImages;
    public Image pin;
    public Destination playerDestination;
    public TeleportDestination teleportDestination;

    [HideInInspector] public bool mapActivated;

    //GameObjetcs for UI Control
    public GameObject gameMaster;
    
    public void OnPlayerMap(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (!mapActivated)
            {
                
                mapActivated = true;
                mapImages.SetActive(true);
                gameMaster.GetComponent<GameMaster>().onOtherMenu = true;//bloqueia o menu de pause quando o mapa estiver ativado
                
                
                // discover player scene 
                if (playerDestination.SceneToGo == "Ciano")
                {
                    pin.rectTransform.position = cyanPos.position;
                }
                Debug.Log("Entrou");
                Debug.Log(teleportDestination.SceneToGo);
                if (playerDestination.SceneToGo == "Lobby")
                {
                    pin.rectTransform.position = whitePos.position;
                }
                
                if (playerDestination.SceneToGo == "Amarelo" )
                {
                    pin.rectTransform.position = yellowPos.position;
                }
                
                if (playerDestination.SceneToGo == "Magenta")
                {
                    pin.rectTransform.position = magentaPos.position;
                }
                
                if (playerDestination.SceneToGo == "Preto")
                {
                    pin.rectTransform.position = blackPos.position;
                }
                
                
            }
            else
            {
                mapImages.SetActive(false);
                mapActivated = false;
                gameMaster.GetComponent<GameMaster>().onOtherMenu = false;
            }
            
        }
        
    }
    void Start()
    {
        mapActivated = false;
        mapImages.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
