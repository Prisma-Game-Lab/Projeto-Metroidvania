using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    public List<GameObject> Colors;
    public GameObject Player;

    private PlayerStatus _playerStatus;
    // Start is called before the first frame update
    void Start()
    {
        _playerStatus = Player.GetComponent<PlayerStatus>();
        SetDoor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ActivateLock(ObjectColor color)
    {
        switch (color)
        {
            case ObjectColor.Magenta:
                _playerStatus.SetColorLock(color);
                Colors[0].SetActive(true);
                break;
            case ObjectColor.Cyan:
                _playerStatus.SetColorLock(color);
                Colors[1].SetActive(true);
                break;
            case ObjectColor.Yellow:
                _playerStatus.SetColorLock(color);
                Colors[2].SetActive(true);
                break;
            case ObjectColor.Black:
                _playerStatus.SetColorLock(color);
                Colors[3].SetActive(true);
                break;
        }
    }

    public void SetDoor()
    {
        foreach(GameObject color in Colors)
        {
            switch (color.GetComponent<ColorComponent>().collectibleColor)
            {
                case ObjectColor.Magenta:
                    if (_playerStatus.lockMagenta)
                    {
                        color.SetActive(true);
                        break;
                    }
                    break;
                case ObjectColor.Cyan:
                    if (_playerStatus.lockCyan)
                    {
                        color.SetActive(true);
                        break;
                    }
                    break;
                case ObjectColor.Yellow:
                    if (_playerStatus.lockYellow)
                    {
                        color.SetActive(true);
                        break;
                    }
                    break;
                case ObjectColor.Black:
                    if (_playerStatus.lockYellow)
                    {
                        color.SetActive(true);
                        break;
                    }
                    break;
            }
        }
    }
}
