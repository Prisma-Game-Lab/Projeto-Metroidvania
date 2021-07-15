using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Cutscene : MonoBehaviour
{
    public VideoPlayer video;
    private double _videoDuration;
    public Animator fade;
    public GameObject firstButtonCutscene;
    public string SceneToLoadNext;

    // Start is called before the first frame update
    void Start()
    {
        fade.SetTrigger("Fade");
        _videoDuration = video.clip.length;
        StartCoroutine(WaitVideo());
        EventSystem.current.SetSelectedGameObject(firstButtonCutscene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkipCutscene()
    {
        StartCoroutine(WaitFade());
    }

    private IEnumerator WaitVideo()
    {
        yield return new WaitForSeconds((float)_videoDuration-1f);
        //fade.gameObject.SetActive(true);
        fade.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneToLoadNext);
    }

    private IEnumerator WaitFade()
    {
        video.Stop();
        fade.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneToLoadNext);
    }

}
