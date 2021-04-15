using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ShurikenSkill : MonoBehaviour
{
    public bool obtained = false;
    public Sprite shurikenSprite;


    private PlayerStatus _playerStatus;



    private void Start()
    {
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
    }

    public void OnShurikenSkill(InputAction.CallbackContext ctx)
    {
        // apertou e não e barco
        if (ctx.started && obtained)
        {
            if(_playerStatus.playerState != PlayerSkill.ShurikenMode)
            {
                _playerStatus.playerState = PlayerSkill.ShurikenMode;
                //_playerStatus.sr.color = Color.magenta;
                _playerStatus.sr.sprite = shurikenSprite;
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
            }
            else
            {
                _playerStatus.playerState = PlayerSkill.Normal;
                //_playerStatus.sr.color = Color.white;
                _playerStatus.sr.sprite = _playerStatus.normalSprite;

                _playerStatus.rb.gravityScale = _playerStatus.playerGravity;
            }

        }
    }
    
    
    
    // Função auxiliar para monstrar a área do ataque
    // private void OnDrawGizmosSelected()
    // {
    //
    //     Vector3 position = transform.position;
    //     Vector2 hitPosition = new Vector2(position.x + _collider.bounds.size.x / 2, position.y);
    //
    //     Gizmos.DrawWireSphere(hitPosition, detectGroundRange);
    // }
}
