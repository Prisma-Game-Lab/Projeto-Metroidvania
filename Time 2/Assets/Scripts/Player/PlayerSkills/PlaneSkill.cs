using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneSkill : MonoBehaviour
{
    private SpriteRenderer _sr;

    public bool obtained = false;
    public Sprite planeSprite;

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
    }

    /*public void OnPlaneSkill()
    {
        PlayerSkill playerState = gameObject.GetComponent<PlayerStatus>().playerState;
        // apertou e nao e barco

        if (obtained)
        {
            if (playerState != PlayerSkill.PlaneMode)
            {
                gameObject.GetComponent<PlayerStatus>().playerState = PlayerSkill.PlaneMode;
                _sr.color = Color.yellow;
            }
            else if (playerState == PlayerSkill.PlaneMode)
            {
                gameObject.GetComponent<PlayerStatus>().playerState = PlayerSkill.Normal;
                _sr.color = Color.white;
            }

        }

    }*/
}
