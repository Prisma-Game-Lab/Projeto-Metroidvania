using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneSkill : MonoBehaviour
{
    

    public bool obtained = false;
    public Sprite planeSprite;
    public float planeUpVelocity; 

    private PlayerStatus _playerStatus ;
    private ParticleSystem _trasformationParticles;
    private float _planeGravity;
    
    
    private void Start()
    {
        _playerStatus = gameObject.GetComponent<PlayerStatus>();
        _trasformationParticles = _playerStatus.transformationParticles.GetComponent<ParticleSystem>();
        _planeGravity = gameObject.GetComponent<PlayerMovement>().flightGravity;
    }

     public void OnPlaneSkill(InputAction.CallbackContext ctx)
     {
         // if player are constrained cant change form 
         if (_playerStatus.isTight || _playerStatus.playerDamage.cantChange)
         {
             // play constrained sound effect 
             return;
         }
             
         
         if (ctx.started && obtained)
         {
             AudioManager.instance.Play("Transform");
             _playerStatus.StopAllTransformationSounds();
             if(_playerStatus.playerState != PlayerSkill.PlaneMode)
             {
                 _trasformationParticles.Play();//liga particulas de transformacao
                 _playerStatus.playerState = PlayerSkill.PlaneMode;
                 // BALL MOVE ANIMATION 
                 _playerStatus.playerAnimator.enabled = true;
                 transform.rotation = Quaternion.identity;
                 _playerStatus.playerCollider.enabled = true;
                 _playerStatus.playerCircleCollider.enabled = false;
                 //
                 
                 _playerStatus.rb.gravityScale = _planeGravity;

                 _playerStatus.sr.sprite = planeSprite;
                 Vector3 v = _playerStatus.sr.bounds.size;
                 BoxCollider2D b = _playerStatus.playerCollider ;
                 b.size = v;
                 
                 _playerStatus.playerMovement._jumpbreak = false;
                 _playerStatus.playerAnimator.SetTrigger("PlaneTrigger");
                 _playerStatus.playerAnimator.SetBool("Player", false);
                 AudioManager.instance.Play("Aviao");
                 
                 if (_playerStatus.rb.velocity.y < -5f)
                 {
                     _playerStatus.rb.velocity = new Vector2(_playerStatus.rb.velocity.x, 0f);
                 }

                 if (_playerStatus.rb.velocity.y > 1f)
                 {
                     // COMPORTAMENTO DE VELOCIDADE DO AVIAO 
                     _playerStatus.rb.velocity = new Vector2(_playerStatus.rb.velocity.x, 0f);
                     _playerStatus.rb.AddForce(new Vector2(0f, planeUpVelocity),ForceMode2D.Impulse);
                 }
                 


                 
             }
             else
             {
                 _playerStatus.SetToNormalState();
                 _playerStatus.playerAnimator.SetBool("Player", true);
                 _playerStatus.playerAnimator.SetTrigger("PlayerTrigger");
             }

         }

     }
}
