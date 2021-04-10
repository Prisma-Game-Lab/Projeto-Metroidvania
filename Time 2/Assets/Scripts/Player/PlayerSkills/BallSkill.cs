using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallSkill : MonoBehaviour
{

    public float ballGravity;
    public bool obtained = false;
    public float reduceFactor;
    

    private PlayerStatus _playerStatus;

    private void Start()
    {
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
    }

    public void OnBallSkill(InputAction.CallbackContext ctx)
    {
        // apertou e não e barco
        if (ctx.started && obtained)
        {
            if(_playerStatus.playerState != PlayerSkill.BallMode)
            {
                _playerStatus.playerState = PlayerSkill.BallMode;
                 _playerStatus.rb.gravityScale = ballGravity;
                 _playerStatus.sr.color = Color.gray;
                
                // change player size and colider 
                 _playerStatus.playerTransform.localScale *= reduceFactor;
            }
            else
            {
                _playerStatus.SetToNormalState();
            }

        }

    }

}