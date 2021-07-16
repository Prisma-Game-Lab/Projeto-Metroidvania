using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLanguageComponent : MonoBehaviour
{

    public Image englishImage;
    public Image portugueseImage;

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
                englishImage.gameObject.SetActive(true);
                break;
            case Idioms.Portuguese:
                portugueseImage.gameObject.SetActive(true);
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
                englishImage.gameObject.SetActive(true);
                portugueseImage.gameObject.SetActive(false);
                break;
            case Idioms.Portuguese:
                portugueseImage.gameObject.SetActive(true);
                englishImage.gameObject.SetActive(false);
                break;
        }
    }
}
