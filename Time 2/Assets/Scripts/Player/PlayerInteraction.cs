using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour
{

    public GameObject UIMaster;
    public GameObject GameMaster;
    public float unlockTextTime;
    [HideInInspector] public bool onDoor = false;
    [HideInInspector] public bool onMailBox = false;
    // life statue interaction 
    [HideInInspector] public bool onFillHeartSTatue = false;
    [HideInInspector] public bool onAddHeartStatue = false;
    
    //Skill Statue Interaction
    [HideInInspector] public bool onSkillStatue = false;

    private GameObject _mailBoxObject;
    private int _statueId;
    private PlayerVictory _playerVictory;
    private PlayerStatus _playerStatus;
    private MailBox _mailBox;
    private UIMaster _uiMaster;
    private SkillStatue _skillStatue;
    private ObjectColor _lockColor;
    private GameObject _finalDoor;
    private StatueData _fillHeart;

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
                AudioManager.instance.Play("Porta");
                if (_playerVictory.CollectedAll())
                {
                    _uiMaster.InteractionPanel.SetActive(false);
                    _uiMaster.PlayerWon(true);
                }
                else
                {
                    if(_lockColor != ObjectColor.None)
                    {
                        //_uiMaster.DoorLockPanel.SetActive(false);
                        _finalDoor.GetComponent<FinalDoor>().ActivateLock(_lockColor);
                        _lockColor = ObjectColor.None;
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
                GameMaster.GetComponent<GameMaster>().ToogleUIActions();
                AudioManager.instance.Play("Carimbo");
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
                AudioManager.instance.Play("Interaction");
                _playerStatus.playerLifeStatue.AddNewHeart(_statueId);
            }else if (onFillHeartSTatue)
            {
                _playerStatus.playerLifeStatue.FillLife();
                _fillHeart.interactionParticle.Play();
                AudioManager.instance.Play("Restaura_Vida");
            }
            else if(onSkillStatue)
            {
                AudioManager.instance.Play("Interaction");
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
            _lockColor = CheckLock();
            _finalDoor = collision.gameObject;
            if (_lockColor != ObjectColor.None)
            {
                _uiMaster.ShowLockPanel(_playerStatus.controlValue);
            }
            else if (_playerVictory.CollectedAll())
            {
                _uiMaster.ShowOpenDoorPanel(_playerStatus.controlValue);
            }
            else
            {
                _uiMaster.ShowInteractionPanel(_playerStatus.controlValue);
            } 
        }
        else if(collision.CompareTag("MailBox")){
            _uiMaster.ShowInteractionPanel(_playerStatus.controlValue);
        }
        else if (collision.CompareTag("SkillStatue"))
        {
            _uiMaster.ShowInteractionPanel(_playerStatus.controlValue);
        }
        else if (collision.CompareTag("FillLifeStatue"))
        {
            _uiMaster.ShowHealPanel(_playerStatus.controlValue);
            _fillHeart = collision.gameObject.GetComponent<StatueData>();
        }
        else if (collision.CompareTag("NewHeartStatue"))
        {
            _uiMaster.ShowAddHeartPanel(_playerStatus.controlValue);
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
        if(_uiMaster.DoorLockPanel != null)
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
        float actualUnlockTime = 0f;
        if (!_playerVictory.CollectedAll())
        {
            _uiMaster.ShowUnLockText();
            actualUnlockTime = unlockTextTime + 1f;
        }   
        else
        {
            _uiMaster.ShowLastInkPanel();
            actualUnlockTime = unlockTextTime;
        }
        gameObject.GetComponent<PlayerInput>().DeactivateInput();
        yield return new WaitForSeconds(unlockTextTime);//tempo para o texto aparecer
        gameObject.GetComponent<PlayerInput>().ActivateInput();
        _uiMaster.DoorLockPanel.SetActive(false);
        _lockColor = CheckLock();
        if (_lockColor != ObjectColor.None)
        {
            _uiMaster.ShowLockPanel(_playerStatus.controlValue);
        }
        else if (_playerVictory.CollectedAll())
        {
            _uiMaster.ShowOpenDoorPanel(_playerStatus.controlValue);

        }
        else
            _uiMaster.ShowInteractionPanel(_playerStatus.controlValue);
    }

    private IEnumerator WaitInkMessage()
    {
        yield return new WaitForSeconds(1f);
        _uiMaster.ShowOpenDoorPanel(_playerStatus.controlValue);
    }
}
