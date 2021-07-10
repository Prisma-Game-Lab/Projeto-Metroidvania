using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour
{

    public GameObject UIMaster;
    public GameObject GameMaster;
    public float unlockAnimationTime;
    private GameObject _mailBoxObject;
    [HideInInspector] public bool onDoor = false;
    [HideInInspector] public bool onMailBox = false;
    // life statue interaction 
    [HideInInspector] public bool onFillHeartSTatue = false;
    [HideInInspector] public bool onAddHeartStatue = false;
    //Skill Statue Interaction
    [HideInInspector] public bool onSkillStatue = false;
    private int _statueId;
    private PlayerVictory _playerVictory;
    private PlayerStatus _playerStatus;
    private MailBox _mailBox;
    private UIMaster _uiMaster;
    private SkillStatue _skillStatue;
    private ObjectColor _lockColor = ObjectColor.None;
    private GameObject _finalDoor;
    private bool _unlocking = false;

    void Start()
    {
        _playerVictory = GetComponent<PlayerVictory>();
        _playerStatus = GetComponent<PlayerStatus>();
        _uiMaster = UIMaster.GetComponent<UIMaster>();
        _lockColor = ObjectColor.None;


    }

    public void Interaction(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (onDoor)
            {
                if (_playerVictory.CollectedAll())
                {
                    _uiMaster.InteractionPanel.SetActive(false);
                    _uiMaster.PlayerWon(true);
                }
                else
                {
                    if(_lockColor != ObjectColor.None)
                    {
                        _uiMaster.DoorLockPanel.SetActive(false);
                        _finalDoor.GetComponent<FinalDoor>().ActivateLock(_lockColor);
                        _lockColor = ObjectColor.None;
                        _unlocking = true;
                        StartCoroutine(UnlockColor());
                        
                    }
                    else
                    {
                        _uiMaster.InteractionPanel.SetActive(false);
                        _uiMaster.PlayerWon(false);
                    }

                }
            }
            else if (onMailBox)
            {
                _uiMaster.InteractionPanel.SetActive(false);
                _mailBox.mailBoxUI.SetActive(true);
                GameMaster.GetComponent<GameMaster>().onOtherMenu = true;
                EventSystem.current.SetSelectedGameObject(_mailBox.mailBoxUIFirstButton);
                this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.PlayerInUI);
                GameMaster.GetComponent<PlayerInput>().SwitchCurrentActionMap(_playerStatus.PlayerInUI);
                //Time.timeScale = 0f;
            }
            else if (onAddHeartStatue)
            {
                _playerStatus.playerLifeStatue.AddNewHeart(_statueId);
            }else if (onFillHeartSTatue)
            {
                _playerStatus.playerLifeStatue.FillLife();
            }
            else if(onSkillStatue)
            {
                _playerStatus.SetPlayerSkill(_skillStatue.skill, _skillStatue.helpDescription.text);
                _uiMaster.InteractionPanel.SetActive(false);
                _uiMaster.ShowSkillDescription(_skillStatue.helpDescription.text);
            }
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FinalDoor"))
        {
            Debug.Log("Na porta");
            //verificar se existem cores a ser colocadas na porta
            //se sim, tela de colocar cor
            _lockColor = CheckLock();
            _finalDoor = collision.gameObject;
            if (_lockColor != ObjectColor.None)
            {
                //exibir tela que oferece interação para colocar cor na porta
                Debug.Log("Na porta depois de pegar cor");
                _uiMaster.ShowLockPanel(_playerStatus.controlValue);
            }
            else//se não tela de interação
            {
                //_uiMaster.InteractionPanel.SetActive(true);
                _uiMaster.ShowInteractionPanel(_playerStatus.controlValue);
            }
            
            
            
        }
        else if(collision.CompareTag("MailBox")){
            //_uiMaster.InteractionPanel.SetActive(true);
            _uiMaster.ShowInteractionPanel(_playerStatus.controlValue);
        }
        else if (collision.CompareTag("SkillStatue") || collision.CompareTag("FillLifeStatue") || collision.CompareTag("NewHeartStatue"))
        {
            //_uiMaster.InteractionPanel.SetActive(true);
            _uiMaster.ShowInteractionPanel(_playerStatus.controlValue);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MailBox"))
        {
            onMailBox = true;
            _mailBoxObject = collision.gameObject;
            _mailBox = _mailBoxObject.GetComponent<MailBox>();
            _mailBox.GetStamp(_mailBox.myMailBox);
        }
        /*else if (collision.CompareTag("FinalDoor"))
        {
            /*_lockColor = CheckLock();
            _finalDoor = collision.gameObject;
            if (_lockColor != ObjectColor.None && !_unlocking)
            {
                //exibir tela que oferece interação para colocar cor na porta
                _uiMaster.ShowLockPanel(_playerStatus.controlValue);
                Debug.Log("Na porta depois de pegar cor");
            }*/
        //}*/

        else if (collision.CompareTag("FillLifeStatue"))
        {
            onFillHeartSTatue = true;
        }
        
        else if (collision.CompareTag("NewHeartStatue"))
        {
            _statueId = collision.GetComponent<StatueData>().id;
            onAddHeartStatue = true;
        }

        else if (collision.CompareTag("SkillStatue"))
        {
            onSkillStatue = true;
            _skillStatue = collision.gameObject.GetComponent<SkillStatue>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onMailBox = false;
        _uiMaster.InteractionPanel.SetActive(false);
        onFillHeartSTatue = false;
        onAddHeartStatue = false;
        onSkillStatue = false;
        _uiMaster.DoorLockPanel.SetActive(false);
        _uiMaster.DisableSkillDescription();

    }

    public bool IsInteracting()
    {
        return onMailBox || onDoor || onAddHeartStatue || onSkillStatue || onFillHeartSTatue;
    }

    public ObjectColor CheckLock()
    {
        if (_playerStatus.magenta && !_playerStatus.lockMagenta)//coletou a cor, mas nao ativou na fechadura
        {
            return ObjectColor.Magenta;
        }
        else if (_playerStatus.cyan && !_playerStatus.lockCyan)//coletou a cor, mas nao ativou na fechadura
        {
            return ObjectColor.Cyan;
        }
        else if (_playerStatus.yellow && !_playerStatus.lockYellow)//coletou a cor, mas nao ativou na fechadura
        {
            return ObjectColor.Yellow;
        }
        else if (_playerStatus.black && !_playerStatus.lockBlack)//coletou a cor, mas nao ativou na fechadura
        {
            return ObjectColor.Black;
        }
        else//todas as cores coletadas ja estao na fechadura
            return ObjectColor.None;
    }

    public IEnumerator UnlockColor()
    {
        //_uiMaster.ShowUnLockText();
        yield return new WaitForSeconds(unlockAnimationTime);
        _unlocking = false;
        _lockColor = CheckLock();
        if (_lockColor != ObjectColor.None)
            _uiMaster.ShowLockPanel(_playerStatus.controlValue);
        else
            _uiMaster.ShowInteractionPanel(_playerStatus.controlValue);
    }
}
