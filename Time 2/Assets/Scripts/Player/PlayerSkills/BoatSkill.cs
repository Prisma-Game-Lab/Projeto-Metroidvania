using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatSkill : MonoBehaviour
{

    private PlayerStatus _playerStatus;
    
    public bool obtained = false;
    public Sprite boatSprite;

    private void Start()
    {
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
    }

    public void OnBoatSkill(InputAction.CallbackContext ctx)
    {
        ;
        // apertou e não e barco

        if (ctx.started && obtained)
        {
            if(_playerStatus.playerState != PlayerSkill.BoatMode)
            {
                _playerStatus.playerState = PlayerSkill.BoatMode;
                // flip tem que se manter 
                if (_playerStatus.playerMovement.isFlipped)
                {
                    Vector3 flippedScale = _playerStatus.originalLocalScale;
                    flippedScale.x *= -1f;
                    _playerStatus.playerTransform.localScale = flippedScale;
                }
                else
                {
                    _playerStatus.playerTransform.localScale = _playerStatus.originalLocalScale;
                }
                _playerStatus.rb.gravityScale = _playerStatus.playerGravity;
                //_playerStatus.sr.color = Color.blue;
                _playerStatus.sr.sprite = boatSprite;
            }
            else
            {
                gameObject.GetComponent<PlayerStatus>().playerState = PlayerSkill.Normal;
                //_playerStatus.sr.color = Color.white;
                _playerStatus.sr.sprite = _playerStatus.normalSprite;
            }

        }

    }

}





