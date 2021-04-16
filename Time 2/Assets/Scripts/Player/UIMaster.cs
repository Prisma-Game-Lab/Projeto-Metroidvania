using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaster : MonoBehaviour
{

    public GameObject HelpSkillText;
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
        Fade.SetActive(true);
        _UILife = life.life;
        itemDescription.description = "";
        _UIItemDescription = itemDescription.description;
        _sceneToGo = playerDestination.SceneToGo;
        _UILifes = new List<GameObject>();
        CreateLifeIcon(_UILife);
    }

    public void ShowSkillDescription(string description)
    {
        HelpSkillText.GetComponent<Text>().text = description;
        HelpSkillText.SetActive(true);
        StartCoroutine(DisableHelperText());
    }
    
    private IEnumerator DisableHelperText()
    {
        yield return new WaitForSeconds(5.0f);
        HelpSkillText.SetActive(false);
    }
    
    
    public void KillPlayer()
    {
        GameOverText.SetActive(true);
        Destroy(gameObject);
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
}
