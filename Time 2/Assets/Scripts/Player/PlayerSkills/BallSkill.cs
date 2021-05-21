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
    

    private PlayerStatus _playerStatus;
    private ParticleSystem _trasformationParticles;

    private void Start()
    {
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _trasformationParticles = _playerStatus.transformationParticles.GetComponent<ParticleSystem>();
    }

    public void OnBallSkill(InputAction.CallbackContext ctx)
    {
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
                Vector3 v = _playerStatus.sr.bounds.size;
                _playerStatus.playerCollider.enabled = false ;
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