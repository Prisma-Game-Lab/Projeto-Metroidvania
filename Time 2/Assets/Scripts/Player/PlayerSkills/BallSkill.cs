using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallSkill : MonoBehaviour
{

    public float ballGravity;
    public bool obtained = false;
    public float reduceFactor;
    public Sprite ballSprite;
    public GameObject attackSlash;
    

    private PlayerStatus _playerStatus;
    private ParticleSystem _trasformationParticles;

    private void Start()
    {
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _trasformationParticles = _playerStatus.transformationParticles.GetComponent<ParticleSystem>();
    }

    public void OnBallSkill(InputAction.CallbackContext ctx)
    {
        // if player are constrained cant change form 
        if(_playerStatus.isTight)
        {
            // PLaytight soundeffect 
            return;
        }
        
        // apertou e não e barco
        if (ctx.started && obtained)
        {
            AudioManager.instance.Play("Transform");
            if(_playerStatus.playerState != PlayerSkill.BallMode)
            {
                _trasformationParticles.Play();//liga particulas de transformacao
                _playerStatus.playerState = PlayerSkill.BallMode;
                 _playerStatus.rb.gravityScale = ballGravity;
                 //_playerStatus.sr.color = Color.gray;
                _playerStatus.sr.sprite = ballSprite;
                // _playerStatus.playerCollider.enabled = false ;

                Vector3 v = _playerStatus.sr.bounds.size;
                BoxCollider2D b = _playerStatus.playerCollider as BoxCollider2D;
                b.size = v * 0.5f;
                _playerStatus.playerCircleCollider.enabled = true;
                _playerStatus.playerCircleCollider.radius = v.x * 0.5f;
                // change player size and colider 
                 _playerStatus.playerTransform.localScale *= reduceFactor;
                 // BALL MOVE ANIMATION 
                 _playerStatus.playerAnimator.enabled = true;
                 transform.rotation = Quaternion.identity;
                 //
                 _playerStatus.playerAnimator.SetBool("Player", false);
                 _playerStatus.playerAnimator.SetBool("Ball", true);
                 _playerStatus.playerAnimator.enabled = false;
                 attackSlash.SetActive(false);
            }
            else
            {
                _playerStatus.SetToNormalState();
                _playerStatus.playerAnimator.SetBool("Ball", false);
                _playerStatus.playerAnimator.SetBool("Player", true);
                _playerStatus.playerAnimator.SetTrigger("PlayerTrigger");
            }

        }

    }

}