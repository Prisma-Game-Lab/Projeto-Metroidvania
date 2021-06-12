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
    private ParticleSystem _trasformationParticles;



    private void Start()
    {
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _trasformationParticles = _playerStatus.transformationParticles.GetComponent<ParticleSystem>();
    }

    public void OnShurikenSkill(InputAction.CallbackContext ctx)
    {
        // if player are constrained cant change form 
        if (_playerStatus.isTight || _playerStatus.playerDamage.cantChange)
        {
            // play constrained sound effect 
            return;
        }
            
        // apertou e não e barco
        if (ctx.started && obtained)
        {
            AudioManager.instance.Play("Transform");
            if(_playerStatus.playerState != PlayerSkill.ShurikenMode)
            {
                _trasformationParticles.Play();
                _playerStatus.playerState = PlayerSkill.ShurikenMode;
                
                // BALL MOVE ANIMATION 
                _playerStatus.playerAnimator.enabled = true;
                transform.rotation = Quaternion.identity;
                _playerStatus.playerCollider.enabled = true;
                _playerStatus.playerCircleCollider.enabled = false;
                //
                
                //_playerStatus.sr.color = Color.magenta;
                _playerStatus.sr.sprite = shurikenSprite;
                Vector3 v = _playerStatus.sr.bounds.size;
                BoxCollider2D b = _playerStatus.playerCollider ;
                b.size = v;
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
                
                
                _playerStatus.playerAnimator.SetBool("Player", false);
                _playerStatus.playerAnimator.SetTrigger("ShurikenTrigger");
            }
            else
            {

                _trasformationParticles.Play();//liga particulas de transformacao
                _playerStatus.playerState = PlayerSkill.Normal;
                //_playerStatus.sr.color = Color.white;
                _playerStatus.sr.sprite = _playerStatus.normalSprite;
                
                Vector3 v = _playerStatus.sr.bounds.size;
                BoxCollider2D b = _playerStatus.playerCollider as BoxCollider2D;
                b.size = v;
                

                _playerStatus.rb.gravityScale = _playerStatus.playerGravity;
                _playerStatus.playerAnimator.SetBool("Player", true);
                _playerStatus.playerAnimator.SetTrigger("PlayerTrigger");
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
