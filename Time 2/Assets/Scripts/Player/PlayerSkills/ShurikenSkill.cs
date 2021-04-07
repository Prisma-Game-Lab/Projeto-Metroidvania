using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ShurikenSkill : MonoBehaviour
{
    public bool obtained = false;

    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private PlayerStatus _playerStatus;
    private Collider2D _collider;
    private PlayerMovement _playerMovement;
    private Transform _playerTransform;
    private Vector3 _originalLocalScale;
    private float _playerGravity;

    private void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _collider = gameObject.GetComponent<CircleCollider2D>();
        _playerGravity = _rb.gravityScale;
        _playerTransform = transform;
        _originalLocalScale = _playerTransform.localScale;
    }

    public void OnShurikenSkill(InputAction.CallbackContext ctx)
    {
        // apertou e não e barco
        if (ctx.started && obtained)
        {
            if(_playerStatus.playerState != PlayerSkill.ShurikenMode)
            {
                _playerStatus.playerState = PlayerSkill.ShurikenMode;
                _sr.color = Color.magenta;
                _playerTransform.localScale = _originalLocalScale;
                _rb.gravityScale = _playerGravity;
                // change player size and colider 
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
