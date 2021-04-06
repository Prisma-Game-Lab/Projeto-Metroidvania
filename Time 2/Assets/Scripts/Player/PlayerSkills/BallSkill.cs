using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallSkill : MonoBehaviour
{

    public float ballGravity;
    public bool obtained = false;
    public float reduceFactor;
    
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private PlayerStatus _playerStatus;
    private Collider2D _collider;
    private Transform _playerTransform;
    private Vector3 _originalLocalScale;
    private float _playerGravity;

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _collider = gameObject.GetComponent<CircleCollider2D>();
        _playerGravity = _rb.gravityScale;
        _playerTransform = transform;
        _originalLocalScale = _playerTransform.localScale;
    }

    public void OnBallSkill(InputAction.CallbackContext ctx)
    {
        // apertou e não e barco
        if (ctx.started && obtained)
        {
            if(_playerStatus.playerState != PlayerSkill.BallMode)
            {
                _playerStatus.playerState = PlayerSkill.BallMode;
                _rb.gravityScale = ballGravity;
                _sr.color = Color.gray;
                
                // change player size and colider 
                _playerTransform.localScale *= reduceFactor;
            }
            else
            {
                _playerStatus.playerState = PlayerSkill.Normal;
                _sr.color = Color.white;
                _playerTransform.localScale = _originalLocalScale;
                _rb.gravityScale = _playerGravity;
            }

        }

    }

}