using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    public VideoPlayer video;
    private double _videoDuration;
    public Animator fade;

    // Start is called before the first frame update
    void Start()
    {
        _videoDuration = video.clip.length;
        StartCoroutine(WaitVideo());
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
        fade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Lobby");
    }

    private IEnumerator WaitFade()
    {
        video.Stop();
        fade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Lobby");
    }

}
