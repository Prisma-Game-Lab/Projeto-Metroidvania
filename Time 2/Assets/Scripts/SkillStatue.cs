using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillStatue : MonoBehaviour
{
    // skill que o item vai dar para o player
    public PlayerSkill skill;
    public ParticleSystem idleParticle;
    [HideInInspector]public Text helpDescription;
    public List<LanguageComponent> SkillTexts;
    public GameObject Player;
    private PlayerStatus _playerStatus;
    // Start is called before the first frame update
    void Start()
    {
        _playerStatus = Player.GetComponent<PlayerStatus>();
        helpDescription = gameObject.GetComponent<Text>();
        SetText(_playerStatus.controlValue);
    }

    // Update is called once per frame
    void Update()
    {
        SetText(_playerStatus.controlValue);
    }

    public void SetText(int controlValue)
    {
        switch (controlValue)
        {
            case 0://teclado1
                helpDescription.text = SkillTexts[0].rightText;
                break;
            case 1://teclado2
                helpDescription.text = SkillTexts[1].rightText;
                break;
            case 2://xbox
                helpDescription.text = SkillTexts[2].rightText;
                break;
            case 3://ps
                helpDescription.text = SkillTexts[3].rightText;
                break;
        }
    }
}
