using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatSkill : MonoBehaviour
{

    public bool obtained = false;
    public Sprite boatSprite;

    private PlayerStatus _playerStatus;
    private ParticleSystem _trasformationParticles;

    private void Start()
    {
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _trasformationParticles = _playerStatus.transformationParticles.GetComponent<ParticleSystem>();
    }

    public void OnBoatSkill(InputAction.CallbackContext ctx)
    {
        // if player are constrained cant change form 
        if (_playerStatus.isTight)
        {
            // PLaytight soundeffect 
            return;
        }
            
        // apertou e não e barco

        if (ctx.started && obtained)
        {
            AudioManager.instance.Play("Transform");
            if(_playerStatus.playerState != PlayerSkill.BoatMode)
            {
                _playerStatus.playerState = PlayerSkill.BoatMode;
                // make animation 
                // BALL MOVE ANIMATION 
                _playerStatus.playerAnimator.enabled = true;
                transform.rotation = Quaternion.identity;
                _playerStatus.playerCollider.enabled = true;
                _playerStatus.playerCircleCollider.enabled = false;
                //
                
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
                _trasformationParticles.Play();//liga particulas de transformacao
                _playerStatus.rb.gravityScale = _playerStatus.playerGravity;
                //_playerStatus.sr.color = Color.blue;
                _playerStatus.sr.sprite = boatSprite;
                Vector3 v = _playerStatus.sr.bounds.size;
                BoxCollider2D b = _playerStatus.playerCollider as BoxCollider2D;
                b.size = v;
                
                
                _playerStatus.playerAnimator.SetTrigger("BoatTrigger");
                _playerStatus.playerAnimator.SetBool("Boat", true);
                _playerStatus.playerAnimator.SetBool("Player", false);
            }
            else
            {
                _trasformationParticles.Play();//liga particulas de transformacao
                gameObject.GetComponent<PlayerStatus>().playerState = PlayerSkill.Normal;
                //_playerStatus.sr.color = Color.white;
                _playerStatus.sr.sprite = _playerStatus.normalSprite;
                
                Vector3 v = _playerStatus.sr.bounds.size;
                BoxCollider2D b = _playerStatus.playerCollider as BoxCollider2D;

                b.size = v;
                
                
                // make animation 
                _playerStatus.playerAnimator.SetBool("Boat", false);
                _playerStatus.playerAnimator.SetBool("Player", true);
                _playerStatus.playerAnimator.SetTrigger("PlayerTrigger");
            }

        }

    }

}





