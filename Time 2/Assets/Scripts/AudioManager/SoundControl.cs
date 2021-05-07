using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public Slider backgroundSlider;
    public Slider soundEffectsSlider;

    private static string _firstGame = "FirstGame";
    private static string _soundEffectsPrefs = "SoundEffectsPrefs";
    private static string _backgroundPrefs = "Backgroundprefs";
    private float _soundEffectsValue;
    private float _backgroundValue;
    private int _firstGameValue;

    void Start()
    {
        _firstGameValue = PlayerPrefs.GetInt(_firstGame);
        SetSoundPrefsBackground();
        SetSoundPrefsSoundEffects();
    }

    // Update is called once per frame
    void Update()
    {
     
    }


    void SetSoundPrefsBackground()
    {
        if (_firstGameValue == 0)
        {
            _backgroundValue = 1f;
            backgroundSlider.value = _backgroundValue;
            PlayerPrefs.SetFloat(_backgroundPrefs, _backgroundValue);
            AudioManager.instance.UpdateSoundVolumes();
        }
        else
        {
            Debug.Log("Else");
            _backgroundValue = PlayerPrefs.GetFloat(_backgroundPrefs);
            backgroundSlider.value = _backgroundValue;
            AudioManager.instance.UpdateSoundVolumes();
        }
    }

    void SetSoundPrefsSoundEffects()
    {
        if (_firstGameValue == 0)
        {
            _soundEffectsValue = 1f;
            soundEffectsSlider.value = _soundEffectsValue;
            PlayerPrefs.SetFloat(_soundEffectsPrefs, _soundEffectsValue);
            PlayerPrefs.SetInt(_firstGame, 1);
            AudioManager.instance.UpdateSoundVolumes();
        }
        else
        {
            Debug.Log("Else");
            _soundEffectsValue = PlayerPrefs.GetFloat(_soundEffectsPrefs);
            Debug.Log(_soundEffectsValue);
            soundEffectsSlider.value = _soundEffectsValue;
            AudioManager.instance.UpdateSoundVolumes();
        }
    }



    public void SetEffectsValue()
    {
        _soundEffectsValue = soundEffectsSlider.value;
        PlayerPrefs.SetFloat(_soundEffectsPrefs, _soundEffectsValue);
        AudioManager.instance.UpdateSoundVolumes();

    }

    public void SetBackgroundValue()
    {
        _backgroundValue = backgroundSlider.value;
        PlayerPrefs.SetFloat(_backgroundPrefs, _backgroundValue);
        AudioManager.instance.UpdateSoundVolumes();
    }

}
