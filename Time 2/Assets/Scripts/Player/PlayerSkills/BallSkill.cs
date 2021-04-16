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
                BoxCollider2D b = _playerStatus.collider as BoxCollider2D;
                b.size = v;
                
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