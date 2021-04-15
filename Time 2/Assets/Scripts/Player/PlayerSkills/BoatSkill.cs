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
        ;
        // apertou e não e barco

        if (ctx.started && obtained)
        {
            AudioManager.instance.Play("Transform");
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
                _trasformationParticles.Play();//liga particulas de transformacao
                _playerStatus.rb.gravityScale = _playerStatus.playerGravity;
                //_playerStatus.sr.color = Color.blue;
                _playerStatus.sr.sprite = boatSprite;
                Vector3 v = _playerStatus.sr.bounds.size; 

                BoxCollider2D b = _playerStatus.collider as BoxCollider2D;

                b.size = v;
            }
            else
            {
                
                _trasformationParticles.Play();//liga particulas de transformacao
                gameObject.GetComponent<PlayerStatus>().playerState = PlayerSkill.Normal;
                //_playerStatus.sr.color = Color.white;
                _playerStatus.sr.sprite = _playerStatus.normalSprite;
                Vector3 v = _playerStatus.sr.bounds.size; 

                BoxCollider2D b = _playerStatus.collider as BoxCollider2D;

                b.size = v;
            }

        }

    }

}





