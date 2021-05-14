using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaster : MonoBehaviour
{

    public GameObject HelpSkillText;
    public GameObject HelpPanel;
    public GameObject DoorPanel;
    public GameObject LifeHud;
    public GameObject GameOverText;
    public GameObject LifeIcon;
    public PlayerHealth life;
    public ItemDescription itemDescription;

    public Destination playerDestination;

    public GameObject Fade;
    // variavies de monitoramento da UI
    private string _UIItemDescription;
    private string _sceneToGo;
    private int _UILife;
    private List<GameObject> _UILifes;

    private void Start()
    {
        
        _UILife = life.life;
        itemDescription.description = "";
        _UIItemDescription = itemDescription.description;
        _sceneToGo = playerDestination.SceneToGo;
        _UILifes = new List<GameObject>();
        CreateLifeIcon(_UILife);
    }

    private void Awake()
    {
        Fade.SetActive(true);
    }

    public void ShowSkillDescription(string description)
    {
        HelpSkillText.GetComponent<Text>().text = description;
        //HelpSkillText.SetActive(true);
        HelpPanel.SetActive(true);
        StartCoroutine(DisableHelperText());
    }
    
    private IEnumerator DisableHelperText()
    {
        yield return new WaitForSeconds(5.0f);
        HelpPanel.SetActive(false);
    }
    
    
    public void KillPlayer()
    {
        GameOverText.SetActive(true);
        Destroy(gameObject);
    }

    public void PlayerWon(bool openDoor)
    {
        if (openDoor)
        {
            StartCoroutine(WaitDoor1());
        }
        else
        {
            StartCoroutine(WaitDoor2());
        }
        
    }


    // MUDAR IMPLEMENTACAO SOMENTE PARA TESTE
    private void RemoveLife()
    {
        if(life.life <= 0)
        {
            KillPlayer();
        }
        else
        {
            CreateLifeIcon(life.life);
        }
    }

    private void CreateLifeIcon(int actualLife)
    {
        float offset = 50f;
        for (int i = 0; i < actualLife; i++)
        {
            Vector3 position = LifeHud.transform.position;
            position.x += offset * i;
            GameObject Life = Instantiate(LifeIcon, position, LifeHud.transform.rotation);
            Life.transform.SetParent(LifeHud.transform);
            _UILifes.Add(Life);
        }
    }

    private void ReRenderLifes()
    {
        foreach (var life in _UILifes)
        {
            Destroy(life);
        }
    }

    private void Update()
    {
        // verifica se os ScriptableObjects estao mudando OBS: Mudar para eventos
        if (_UILife != life.life)
        {
            _UILife = life.life;
            ReRenderLifes();
            RemoveLife();
        }

        if (_UIItemDescription != itemDescription.description)
        {
            _UIItemDescription = itemDescription.description;
            ShowSkillDescription(_UIItemDescription);
        }

        if (_sceneToGo != playerDestination.SceneToGo)
        {
            _sceneToGo = playerDestination.SceneToGo;
            ChangeSceneEffect();
        }
        
    }

    private void ChangeSceneEffect()
    {
        Fade.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    private IEnumerator WaitDoor1()//mostra o texto da porta quando ha vitoria
    {
        DoorPanel.SetActive(true);
        DoorPanel.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        DoorPanel.transform.GetChild(0).gameObject.SetActive(false);
        DoorPanel.SetActive(false);
    }

    private IEnumerator WaitDoor2()//mostra o texto da porta quando ainda nao foram coletadadas todas as tintas
    {
        DoorPanel.SetActive(true);
        DoorPanel.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        DoorPanel.transform.GetChild(1).gameObject.SetActive(false);
        DoorPanel.SetActive(false);
    }

}
