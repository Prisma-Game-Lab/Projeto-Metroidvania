using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLanguageComponent : MonoBehaviour
{

    public Text englishText;
    public Text portugueseText;

    [HideInInspector] public string rightText;
    public Idioms idioms;
    // Start is called before the first frame update
    void Start()
    {
        int idiom = PlayerPrefs.GetInt("Language");
        if (idiom == 1)
        {
            idioms = Idioms.Portuguese;
        }
        else if (idiom == 2)
        {
            idioms = Idioms.English;
        }

        switch (idioms)
        {
            case Idioms.English:
                englishText.gameObject.SetActive(true);
                //rightText = englishText.text;
                break;
            case Idioms.Portuguese:
                portugueseText.gameObject.SetActive(true);
                //rightText = portugueseText.text;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int idiom = PlayerPrefs.GetInt("Language");
        if (idiom == 1)
        {
            idioms = Idioms.Portuguese;
        }
        else if (idiom == 2)
        {
            idioms = Idioms.English;
        }

        switch (idioms)
        {
            case Idioms.English:
                englishText.gameObject.SetActive(true);
                portugueseText.gameObject.SetActive(false);
                //rightText = englishText.text;
                break;
            case Idioms.Portuguese:
                portugueseText.gameObject.SetActive(true);
                englishText.gameObject.SetActive(false);
                //rightText = portugueseText.text;
                break;
        }
    }
}
