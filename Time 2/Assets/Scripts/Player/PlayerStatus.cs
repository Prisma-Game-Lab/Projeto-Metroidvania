using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;


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
    public bool isTight = false;
    public PlayerAnimationState playerAnimationState = PlayerAnimationState.Idle;
    public ItemDescription itemDescription;
    [HideInInspector] public Vector3 _lastSafePos; // ultima posicao segura para o jogador

    // player Components 
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerDamage playerDamage;
    [HideInInspector] public Animator playerAnimator;
    [HideInInspector] public BoxCollider2D playerCollider;
    [HideInInspector] public CircleCollider2D playerCircleCollider;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Vector3 originalLocalScale;
    [HideInInspector] public float playerGravity;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerLifeStatue playerLifeStatue;
    public GameObject transformationParticles;

    // Fala se o player esta no processo de respawn 
    [HideInInspector]public bool willRespawn = false;
    //Controls
    private static string _controlPrefs = "ControlPrefs";
    private int _controlValue;
    public PlayerHealth playerHealth;
    [HideInInspector] public string PlayerActions;
    [HideInInspector] public string GlobalActions;
    [HideInInspector] public string PlayerInUI;
    [HideInInspector] public string PlayerInMap;

    // Variaveis para serem salvas 
    [HideInInspector] public int totalLife;
    [HideInInspector] public int[] NewHeartsId;
    
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
    [HideInInspector] public bool stampLobby = false;

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
        SetControl();
        //Debug.Log(PlayerActions);
        //Debug.Log(GlobalActions);
        //Debug.Log(PlayerInUI);

    }

    private void Start()
    {

        _lastSafePos = transform.position;
        transformationParticles.GetComponent<ParticleSystem>().Play();//liga particulas//o jogo come�a com o efeito de particulas ligado
        // pegar os componentes do player
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        playerDamage = gameObject.GetComponent<PlayerDamage>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerGravity = rb.gravityScale; // gravidade original do player
        playerAnimator = gameObject.GetComponent<Animator>();
        playerAnimator.SetBool("Player", true);
        playerCircleCollider = gameObject.GetComponent<CircleCollider2D>();
        playerLifeStatue = gameObject.GetComponent<PlayerLifeStatue>();
        // correct player colider 
        Vector3 v = sr.bounds.size;
        BoxCollider2D b = playerCollider;
        b.size = v;
        this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(PlayerActions);
        
        // control the respawn 
        willRespawn = false;


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Verifica a morte pela agua
        if (collision.collider.CompareTag("Water") && (playerState != PlayerSkill.BoatMode) && !willRespawn )
        {
            willRespawn = true;
            gameObject.GetComponent<PlayerDamage>().TakeEnvironmentDamage();
            ReturnPlayerToSafePos();
            return;
        }

        if (collision.collider.CompareTag("Spike")  && !willRespawn)
        {
            willRespawn = true;
            gameObject.GetComponent<PlayerDamage>().TakeEnvironmentDamage();
            ReturnPlayerToSafePos();
        }
    
    }
    
    private void ReturnPlayerToSafePos()
    {
        transform.position = _lastSafePos;
        rb.velocity = Vector2.zero;
        // transformar o player no estado normal
        SetToNormalState();
        StartCoroutine(WaitRespawn());

    }
    
    private IEnumerator WaitRespawn()
    {
        if (playerHealth.life > 0)
        {
            gameObject.GetComponent<PlayerInput>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponent<PlayerInput>().enabled = true;
            willRespawn = false;
        }
        yield return new WaitForSeconds(0.1f);
        SetControl();
        this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(PlayerActions);

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
        
        if(SceneManager.GetActiveScene().name=="Boss 1")
        {
            gameObject.GetComponent<PlayerAttack>().obtained = true;
            sword = true;
            gameObject.GetComponent<BoatSkill>().obtained = true;
            boat = true;
            gameObject.GetComponent<PlaneSkill>().obtained = true;
            airplane = true;
            gameObject.GetComponent<BallSkill>().obtained = true;
            ball = true;
            gameObject.GetComponent<ShurikenSkill>().obtained = true;
            shuriken = true;
            totalLife = playerHealth.totalLife;
            return;
        }
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null)
        {
            totalLife = playerHealth.startLife;
            playerHealth.life = totalLife;
            playerHealth.totalLife = totalLife;
            cyan = false;
            yellow = false; 
            magenta = false;
            black = false;
        
            // Skills 
            boat = false;
            airplane = false;
            sword = false;
            ball = false;
            shuriken = false;

            NewHeartsId = new int[] {};
            
            // liberar as skills de acordo com o save 
            gameObject.GetComponent<BoatSkill>().obtained = false;
            gameObject.GetComponent<PlayerAttack>().obtained = false;
            gameObject.GetComponent<PlaneSkill>().obtained = false;
            gameObject.GetComponent<BallSkill>().obtained = false;
            gameObject.GetComponent<ShurikenSkill>().obtained = false;

            //stamps
            stampMagenta = false;
            stampCyan = false;
            stampYellow = false;
            stampBlack = false;
            stampLobby = false;

            //Teleport Status
            stampTeleport = false;
            return;
        }

        totalLife = data.totalLife;
        playerHealth.life = totalLife;
        playerHealth.totalLife = totalLife;
        
        NewHeartsId = data.newHeartsId;

        //colors 
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
        stampLobby = data.stampLobby;

        //Teleport Status
        stampTeleport = data.stampTeleport;

    }

    public void SetToNormalState()
    {
        // BALL MOVE ANIMATION 
        playerAnimator.enabled = true;
        transform.rotation = Quaternion.identity;
        playerCollider.enabled = true;
        playerCircleCollider.enabled = false;
        //
        // player animations 
        playerAnimator.SetBool("Player", true);
        playerAnimator.SetTrigger("PlayerTrigger");
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
            case stampDestination.lobby:
                stampLobby = true;
                break;
        }
        SaveSystem.SavePlayer(this);
    }

    public void UpdateStampStatus(Stamp stamp)
    {
        switch (stamp.mailBoxToGo)
        {
            case stampDestination.magenta:
                stamp.obtained = stampMagenta;
                break;
            case stampDestination.cyan:
                stamp.obtained = stampCyan;
                break;
            case stampDestination.yellow:
                stamp.obtained = stampYellow;
                break;
            case stampDestination.black:
                stamp.obtained = stampBlack;
                break;
            case stampDestination.lobby:
                stamp.obtained = stampLobby;
                break;

        }

    }

    public void SetTeleportStatus(bool status)
    {
        stampTeleport = status;
        SaveSystem.SavePlayer(this);
    }

    public void SetControl()
    {
        _controlValue = PlayerPrefs.GetInt(_controlPrefs);
        //Debug.Log(_controlValue);
        switch (_controlValue)
        {
            case 0:
                PlayerActions = "PlayerActionsK1";
                GlobalActions = "GlobalActionsKeyboard";
                PlayerInUI = "PlayerInUIKeyboard";
                PlayerInMap = "PlayerInMapKeyboard";
                break;
            case 1:
                PlayerActions = "PlayerActionsK2";
                GlobalActions = "GlobalActionsKeyboard";
                PlayerInUI = "PlayerInUIKeyboard";
                PlayerInMap = "PlayerInMapKeyboard";
                break;
            case 2:
                PlayerActions = "PlayerActionsXbox";
                GlobalActions = "GlobalActionsController";
                PlayerInUI = "PlayerInUIController";
                PlayerInMap = "PlayerInMapController";
                break;
            case 3:
                PlayerActions = "PlayerActionsPS";
                GlobalActions = "GlobalActionsController";
                PlayerInUI = "PlayerInUIController";
                PlayerInMap = "PlayerInMapController";
                break;
        }
    }

}
