using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public enum PlayerSkill
{
    Normal,
    AttackSkill,
    BallMode,
    BoatMode,
    PlaneMode,
    ShurikenMode
}

public class PlayerStatus : MonoBehaviour
{

    public PlayerSkill playerState = PlayerSkill.Normal;
    public ItemDescription itemDescription;
    [HideInInspector] public Vector3 _lastSafePos; // ultima posicao segura para o jogador
    // player Components 
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public Collider2D collider;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Vector3 originalLocalScale;
    [HideInInspector] public float playerGravity;

    // Variaveis para serem salvas 
    // Colors
    [HideInInspector] public bool cyan = false;
    [HideInInspector] public bool yellow = false;
    [HideInInspector] public bool magenta = false;
    [HideInInspector] public bool black = false; 
  
    // Skills 
    [HideInInspector] public bool boat = false;
    [HideInInspector] public bool airplane = false;
    [HideInInspector] public bool sword = false;
    [HideInInspector] public bool ball = false;
    [HideInInspector] public bool shuriken = false;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        // checar se existe player a ser carregado 
        LoadPlayer();
        _lastSafePos = transform.position;
        
        // pegar os componentes do player
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerGravity = rb.gravityScale; // gravidade original do player
        playerTransform = transform;
        originalLocalScale = playerTransform.localScale;
        collider = gameObject.GetComponent<CircleCollider2D>();

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Verifica a morte pela agua
        if (collision.collider.CompareTag("Water") && (playerState != PlayerSkill.BoatMode))
        {
            gameObject.GetComponent<PlayerDamage>().TakeDamage();
            ReturnPlayerToSafePos();
        }

    }

    public void SetPlayerSkill(PlayerSkill skill, string description)
    {
        switch (skill)
        {
            case  PlayerSkill.AttackSkill:
                gameObject.GetComponent<PlayerAttack>().obtained = true;
                sword = true;
                break;
            case PlayerSkill.BoatMode:
                gameObject.GetComponent<BoatSkill>().obtained = true;
                boat = true;
                break;
            case PlayerSkill.Normal:
                break;
            case PlayerSkill.PlaneMode:
                gameObject.GetComponent<PlaneSkill>().obtained = true;
                airplane = true; 
                break;
            case PlayerSkill.BallMode:
                gameObject.GetComponent<BallSkill>().obtained = true;
                ball = true; 
                break;
            case PlayerSkill.ShurikenMode:
                gameObject.GetComponent<ShurikenSkill>().obtained = true;
                shuriken = true; 
                break;
        }

        // Mostrar a descricao de ajuda asobre a habilidade 
        SaveSystem.SavePlayer(this);
        itemDescription.description = description;
    }

    private void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null) return;
        cyan = data.cyan;
        yellow = data.yellow; 
        magenta = data.magenta;
        black = data.black;
        
        // Skills 
        boat = data.boat;
        airplane = data.airplane;
        sword = data.sword;
        ball = data.ball;
        shuriken = data.shuriken;
        
        // liberar as skills de acordo com o save 
        gameObject.GetComponent<BoatSkill>().obtained = boat;
        gameObject.GetComponent<PlayerAttack>().obtained = sword;
        gameObject.GetComponent<PlaneSkill>().obtained = airplane;
        gameObject.GetComponent<BallSkill>().obtained = ball;
        gameObject.GetComponent<ShurikenSkill>().obtained = shuriken;

    }

    private void ReturnPlayerToSafePos()
    {
        transform.position = _lastSafePos;
        // transformar o player no estado normal
        SetToNormalState();
        
    }

    public void SetToNormalState()
    {
        playerState = PlayerSkill.Normal;
        sr.color = Color.white;
                
        // flip tem que se manter 
        if (playerMovement.isFlipped)
        {
            Vector3 flippedScale = originalLocalScale;
            flippedScale.x *= -1f;
            playerTransform.localScale = flippedScale;
        }
        else
        {
            playerTransform.localScale = originalLocalScale;
        }
                
        rb.gravityScale = playerGravity;
    }
}
