using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Idioms
{
    Portuguese,
    English
}
public class LanguageComponent : MonoBehaviour
{
    public Text englishText;
    public Text portugueseText;
    
    [HideInInspector] public string rightText;
    public Idioms idioms;

    private void Start()
    {
        PlayerPrefs.SetInt("Language", 1); // portuguese
        //PlayerPrefs.SetInt("Language", 2); // english
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
                rightText = englishText.text;
                break;
            case Idioms.Portuguese:
                rightText = portugueseText.text;
                break;
        }
    }
}
