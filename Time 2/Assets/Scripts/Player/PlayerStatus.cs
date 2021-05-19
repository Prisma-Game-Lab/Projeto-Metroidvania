using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.InputSystem;
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

public enum PlayerAnimationState
{
    Idle,
    Movement,
    Attack,
}

public class PlayerStatus : MonoBehaviour
{

    public PlayerSkill playerState = PlayerSkill.Normal;
    public PlayerAnimationState playerAnimationState = PlayerAnimationState.Idle;
    public ItemDescription itemDescription;
    [HideInInspector] public Vector3 _lastSafePos; // ultima posicao segura para o jogador

    // player Components 
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public BoxCollider2D playerCollider;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Vector3 originalLocalScale;
    [HideInInspector] public float playerGravity;
    public GameObject transformationParticles;

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

    //Sprites
    public Sprite normalSprite;

    //Stamps
    [HideInInspector] public bool stampMagenta = false;
    [HideInInspector] public bool stampCyan = false;
    [HideInInspector] public bool stampYellow = false;
    [HideInInspector] public bool stampBlack = false;

    //Teleport Status
    [HideInInspector] public bool stampTeleport = false;


    // Start is called before the first frame update

    private void Awake()
    {
        // checar se existe player a ser carregado 
        Time.timeScale = 1f;
        LoadPlayer();
        playerTransform = transform;
        originalLocalScale = playerTransform.localScale;
        playerCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {

        _lastSafePos = transform.position;
        transformationParticles.GetComponent<ParticleSystem>().Play();//liga particulas//o jogo come�a com o efeito de particulas ligado
        // pegar os componentes do player
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerGravity = rb.gravityScale; // gravidade original do player

        // correct player colider 
        Vector3 v = sr.bounds.size;
        BoxCollider2D b = playerCollider;
        b.size = v;
        

}

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Verifica a morte pela agua
        if (collision.collider.CompareTag("Water") && (playerState != PlayerSkill.BoatMode) )
        {
            gameObject.GetComponent<PlayerDamage>().TakeDamage();
            Debug.Log(gameObject.name);
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

        //stamps
        stampMagenta = data.stampMagenta;
        stampCyan = data.stampCyan;
        stampYellow = data.stampYellow;
        stampBlack = data.stampBlack;

        //Teleport Status
        stampTeleport = data.stampTeleport;

    }

    private void ReturnPlayerToSafePos()
    {
        transform.position = _lastSafePos;
        // transformar o player no estado normal
        SetToNormalState();
        StartCoroutine(WaitRespawn());

    }

    public void SetToNormalState()
    {
        transformationParticles.GetComponent<ParticleSystem>().Play();//liga particulas//liga particulas
        playerState = PlayerSkill.Normal;
        //sr.color = Color.white;
        sr.sprite = normalSprite;
        Vector3 v = sr.bounds.size;
        BoxCollider2D b = playerCollider;
        b.size = v;
                
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

    private IEnumerator WaitRespawn()
    {
        gameObject.GetComponent<PlayerInput>().actions.Disable();
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<PlayerInput>().actions.Enable();
    }

    public void SetStampStatus(stampDestination stampMailBox)
    {
        switch (stampMailBox)
        {
            case stampDestination.magenta:
                stampMagenta = true;
                break;
            case stampDestination.cyan:
                stampCyan = true;
                break;
            case stampDestination.yellow:
                stampYellow = true;
                break;
            case stampDestination.black:
                stampBlack = true;
                break;
        }
        SaveSystem.SavePlayer(this);
    }

    public void SetTeleportStatus(bool status)
    {
        stampTeleport = status;
        SaveSystem.SavePlayer(this);
    }

}
