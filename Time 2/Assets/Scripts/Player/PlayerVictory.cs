using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVictory : MonoBehaviour
{
    public List<ObjectColor> _obtainedColors = new List<ObjectColor>();//mudar para private quando acabar de testar
    public GameObject UIMaster;

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
            {
                UIMaster.GetComponent<UIMaster>().PlayerWon();
            }
        }

    }

    public bool CollectedAll()
    {
        if (_obtainedColors.Count == 4)
            return true;
        return false;
    }
}
