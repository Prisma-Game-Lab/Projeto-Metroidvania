using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHUD : MonoBehaviour
{
    public GameObject Player;
    public List<GameObject> ButtonIcons;
    // Start is called before the first frame update
    void Start()
    {
        SetMapHUDButton(Player.GetComponent<PlayerStatus>().controlValue);
    }

    // Update is called once per frame
    void Update()
    {
        SetMapHUDButton(Player.GetComponent<PlayerStatus>().controlValue);
    }

    public void SetMapHUDButton(int controlValue)
    {
        ClearButtons();
        switch (controlValue)
        {
            case 0://teclado1
                ButtonIcons[0].SetActive(true);
                break;
            case 1://teclado2
                ButtonIcons[1].SetActive(true);
                break;
            case 2://xbox
                ButtonIcons[2].SetActive(true);
                break;
            case 3://ps
                ButtonIcons[3].SetActive(true);
                break;
        }
    }

    public void ClearButtons()
    {
        ButtonIcons[0].SetActive(false);
        ButtonIcons[1].SetActive(false);
        ButtonIcons[2].SetActive(false);
        ButtonIcons[3].SetActive(false);
    }
}
