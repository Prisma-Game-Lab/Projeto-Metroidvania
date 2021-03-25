using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatSkill : MonoBehaviour
{
    private SpriteRenderer _sr;

    public bool obtained = false;

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
    }

    public void OnBoatSkill(InputAction.CallbackContext ctx)
    {
        PlayerSkill playerState = gameObject.GetComponent<PlayerStatus>().playerState;
        // apertou e não e barco

        if (ctx.started && obtained)
        {
            if(playerState != PlayerSkill.BoatMode)
            {
                gameObject.GetComponent<PlayerStatus>().playerState = PlayerSkill.BoatMode;
                _sr.color = Color.blue;
            }
            else if(playerState == PlayerSkill.BoatMode)
            {
                gameObject.GetComponent<PlayerStatus>().playerState = PlayerSkill.Normal;
                _sr.color = Color.white;
            }

        }

    }

}
