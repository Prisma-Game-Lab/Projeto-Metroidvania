using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIMaster : MonoBehaviour
{

    public GameObject HelpSkillText;
    public GameObject HelpPanel;
    public GameObject DoorPanel;
    public GameObject LifeHud;
    public GameObject GameOverText;
    public GameObject LifeIcon;
    public GameObject LifeBorder;
    public List<Image> LifeHUDBars;
    public PlayerHealth life;
    public ItemDescription itemDescription;
    public GameObject InteractionPanel;
    public GameObject ColorPanel;
    public GameObject DoorLockPanel;
    public LanguageComponent UnlockColorText;
    public LanguageComponent LastInkText;
    public GameObject DeathUI;
    public GameObject firstDeathButton;

    public Destination playerDestination;

    public GameObject Fade;

    public List<LanguageComponent> InteractionTexts;
    public List<LanguageComponent> LockTexts;
    public List<LanguageComponent> HealTexts;
    public List<LanguageComponent> AddHeartTexts;
    public List<LanguageComponent> OpenDoorTexts;
    // variavies de monitoramento da UI
    private string _UIItemDescription;
    private string _sceneToGo;
    private int _UILife;
    private List<GameObject> _UILifes;
    private int _UILifeBorder;
    private List<GameObject> _UILifeBorders;

    private void Start()
    {
        _UILife = life.life;
        _UILifeBorder = life.totalLife;
        itemDescription.description = "";
        _UIItemDescription = itemDescription.description;
        _sceneToGo = playerDestination.SceneToGo;
        _UILifes = new List<GameObject>();
        CreateLifeIcon(_UILife);
        _UILifeBorders = new List<GameObject>();
        CreateLifeBorder(_UILifeBorder);
        SelectLifeHUDBar();
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
        //StartCoroutine(DisableHelperText());
    }

    public void DisableSkillDescription()
    {
        HelpPanel.SetActive(false);
    }
    
    /*private IEnumerator DisableHelperText()
    {
        yield return new WaitForSeconds(5.0f);
        HelpPanel.SetActive(false);
    }*/
    
    
    public void KillPlayer()
    {
        //GameOverText.SetActive(true);
        DeathUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstDeathButton);
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
            CreateLifeBorder(life.totalLife);
            SelectLifeHUDBar();
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

    private void CreateLifeBorder(int totalLife)
    {
        float offset = 50f;
        for (int i = 0; i < totalLife; i++)
        {
            Vector3 position = LifeHud.transform.position;
            position.x += offset * i;
            GameObject Life = Instantiate(LifeBorder, position, LifeHud.transform.rotation);
            Life.transform.SetParent(LifeHud.transform);
            _UILifeBorders.Add(Life);
        }
    }

    private void ReRenderLifes()
    {
        foreach (var life in _UILifes)
        {
            Destroy(life);
        }
        foreach (var life in _UILifeBorders)
        {
            Destroy(life);
        }
    }

    public void SelectLifeHUDBar()
    {
        int idx = life.totalLife - life.startLife;
        foreach (Image lifeBar in LifeHUDBars)
        {
            lifeBar.gameObject.SetActive(false);
        }
        LifeHUDBars[idx].gameObject.SetActive(true);
    }
    private void Update()
    {
        // verifica se os ScriptableObjects estao mudando OBS: Mudar para eventos
        
        if (_UILife != life.life || _UILifeBorder != life.totalLife)
        {
            ReRenderLifes();
            _UILife = life.life;
            _UILifeBorder = life.totalLife;
            RemoveLife();
        }

        /*if (_UIItemDescription != itemDescription.description)
        {
            _UIItemDescription = itemDescription.description;
            ShowSkillDescription(_UIItemDescription);
        }*/

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
        Fade.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("BossDoorCutscene");
        //DoorPanel.SetActive(true);
        //DoorPanel.transform.GetChild(0).gameObject.SetActive(true);
        
        //DoorPanel.transform.GetChild(0).gameObject.SetActive(false);
        //DoorPanel.SetActive(false);
    }

    private IEnumerator WaitDoor2()//mostra o texto da porta quando ainda nao foram coletadadas todas as tintas
    {
        DoorPanel.SetActive(true);
        DoorPanel.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        DoorPanel.transform.GetChild(1).gameObject.SetActive(false);
        DoorPanel.SetActive(false);
    }

    public void ShowInteractionPanel(int controlValue)
    {
        ShowInteractionText(controlValue);
        InteractionPanel.SetActive(true);
    }

    public void ShowHealPanel(int controlValue)
    {
        ShowHealText(controlValue);
        InteractionPanel.SetActive(true);
    }

    public void ShowLockPanel(int controlValue)
    {
        ShowLockText(controlValue);
        DoorLockPanel.SetActive(true);
    }

    public void ShowAddHeartPanel(int controlValue)
    {
        ShowAddHeartText(controlValue);
        InteractionPanel.SetActive(true);
    }

    public void ShowOpenDoorPanel(int controlValue)
    {
        ShowOpenDoorText(controlValue);
        InteractionPanel.SetActive(true);
    }

    public void ShowInteractionText(int controlValue)
    {
        switch (controlValue)
        {
            case 0://teclado1
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = InteractionTexts[0].rightText;
                break;
            case 1://teclado2
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = InteractionTexts[1].rightText;
                break;
            case 2://xbox
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = InteractionTexts[2].rightText;
                break;
            case 3://ps
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = InteractionTexts[3].rightText;
                break;
        }
    }



    public void ShowLockText(int controlValue)
    {
        switch (controlValue)
        {
            case 0://teclado1
                DoorLockPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = LockTexts[0].rightText;
                break;
            case 1://teclado2
                DoorLockPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = LockTexts[1].rightText;
                break;
            case 2://xbox
                DoorLockPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = LockTexts[2].rightText;
                break;
            case 3://ps
                DoorLockPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = LockTexts[3].rightText;
                break;
        }
    }

    public void ShowUnLockText()
    {
        DoorLockPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = UnlockColorText.rightText;
    }

    public void ShowLastInkText()
    {
        DoorLockPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = LastInkText.rightText;
    }

    public void ShowLastInkPanel()
    {
        ShowLastInkText();
        DoorLockPanel.SetActive(true);
    }

    public void ShowHealText(int controlValue)
    {
        switch (controlValue)
        {
            case 0://teclado1
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = HealTexts[0].rightText;
                break;
            case 1://teclado2
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = HealTexts[1].rightText;
                break;
            case 2://xbox
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = HealTexts[2].rightText;
                break;
            case 3://ps
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = HealTexts[3].rightText;
                break;
        }
    }

    public void ShowAddHeartText(int controlValue)
    {
        switch (controlValue)
        {
            case 0://teclado1
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = AddHeartTexts[0].rightText;
                break;
            case 1://teclado2
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = AddHeartTexts[1].rightText;
                break;
            case 2://xbox
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = AddHeartTexts[2].rightText;
                break;
            case 3://ps
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = AddHeartTexts[3].rightText;
                break;
        }
    }

    public void ShowOpenDoorText(int controlValue)
    {
        switch (controlValue)
        {
            case 0://teclado1
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = OpenDoorTexts[0].rightText;
                break;
            case 1://teclado2
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = OpenDoorTexts[1].rightText;
                break;
            case 2://xbox
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = OpenDoorTexts[2].rightText;
                break;
            case 3://ps
                InteractionPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = OpenDoorTexts[3].rightText;
                break;
        }
    }

    public void ShowColorText()
    {
        ColorPanel.SetActive(true);
        StartCoroutine(WaitColorText());
    }
    private IEnumerator WaitColorText()
    {
        yield return new WaitForSeconds(2f);
        ColorPanel.SetActive(false);

    }
}
