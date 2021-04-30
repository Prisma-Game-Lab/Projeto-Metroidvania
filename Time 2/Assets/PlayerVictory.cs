using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVictory : MonoBehaviour
{
    private List<ObjectColor> _obtainedColors = new List<ObjectColor>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Color"))
        {
            ObjectColor collectedColor = collision.GetComponent<ColorComponent>().collectibleColor;
            if (!_obtainedColors.Contains(collectedColor))
            {
                _obtainedColors.Add(collectedColor);
                Destroy(collision.gameObject);
            }

        }
        else if (collision.CompareTag("FinalDoor"))
        {
            if (CollectedAll())
                //aparece aviso de vitoria;
                Debug.Log("Ganhou");
            /*else
                //avisa que ainda nao pode acessar a porta
            */ 
        }

    }

    public bool CollectedAll()
    {
        if (_obtainedColors.Count == 4)
            return true;
        return false;
    }
}
