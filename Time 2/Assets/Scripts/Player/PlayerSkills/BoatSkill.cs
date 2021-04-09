using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatSkill : MonoBehaviour
{
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private float _playerGravity;
    private Transform _playerTransform;
    private Vector3 _originalLocalScale;
    private PlayerMovement _playerMovement;
    
    public bool obtained = false;

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
        _playerGravity = _rb.gravityScale; // gravidade original do player
        _playerTransform = transform;
        _originalLocalScale = _playerTransform.localScale;
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
                // flip tem que se manter 
                if (_playerMovement.isFlipped)
                {
                    Vector3 flippedScale = _originalLocalScale;
                    flippedScale.x *= -1f;
                    _playerTransform.localScale = flippedScale;
                }
                else
                {
                    _playerTransform.localScale = _originalLocalScale;
                }
                _rb.gravityScale = _playerGravity;
                _sr.color = Color.blue;
            }
            else
            {
                gameObject.GetComponent<PlayerStatus>().playerState = PlayerSkill.Normal;
                _sr.color = Color.white;
            }

        }

    }

}





