using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcons : MonoBehaviour
{
    public List<Image> Icons;
    public GameObject Player;
    private PlayerStatus _playerStatus;
    // Start is called before the first frame update
    void Start()
    {
        _playerStatus = Player.GetComponent<PlayerStatus>();
        SetSkillIcons();
    }

    // Update is called once per frame
    void Update()
    {
        SetSkillIcons();
    }

    public void SetSkillIcons()
    {
        foreach(Image icon in Icons)
        {
            SkillIconComponent skill = icon.gameObject.GetComponent<SkillIconComponent>();
            switch (icon.gameObject.GetComponent<SkillIconComponent>().skill)
            {
                case PlayerSkill.BallMode:
                    if (_playerStatus.ball)
                    {
                        skill.NotObtainedVersion.gameObject.SetActive(false);
                        skill.ObtainedVersion.gameObject.SetActive(true);
                    }
                    else
                    {
                        skill.ObtainedVersion.gameObject.SetActive(false);
                        skill.NotObtainedVersion.gameObject.SetActive(true);
                    }
                    break;
                case PlayerSkill.BoatMode:
                    if (_playerStatus.boat)
                    {
                        skill.NotObtainedVersion.gameObject.SetActive(false);
                        skill.ObtainedVersion.gameObject.SetActive(true);
                    }
                    else
                    {
                        skill.ObtainedVersion.gameObject.SetActive(false);
                        skill.NotObtainedVersion.gameObject.SetActive(true);
                    }
                    break;
                case PlayerSkill.PlaneMode:
                    if (_playerStatus.airplane)
                    {
                        skill.NotObtainedVersion.gameObject.SetActive(false);
                        skill.ObtainedVersion.gameObject.SetActive(true);
                    }
                    else
                    {
                        skill.ObtainedVersion.gameObject.SetActive(false);
                        skill.NotObtainedVersion.gameObject.SetActive(true);
                    }
                    break;
                case PlayerSkill.ShurikenMode:
                    if (_playerStatus.shuriken)
                    {
                        skill.NotObtainedVersion.gameObject.SetActive(false);
                        skill.ObtainedVersion.gameObject.SetActive(true);
                    }
                    else
                    {
                        skill.ObtainedVersion.gameObject.SetActive(false);
                        skill.NotObtainedVersion.gameObject.SetActive(true);
                    }
                    break;
            }
        }
    }
}
