using System;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] soundEffects;
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = PlayerPrefs.GetFloat("Backgroundprefs")*0.005f;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            if (s.name == "Magenta")
            {
                s.source.volume = s.source.volume * 0.3f;
            }
        }
        foreach (Sound s in soundEffects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = PlayerPrefs.GetFloat("SoundEffectsPrefs")*0.005f;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }

    public void Play(string name)
    {
        Sound s =  Array.Find(sounds, sound => sound.name == name);
        if(s!= null)
        {
            s.source.Play();
            return;
        }
        s = Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s =  Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
            return;
        }
        s = Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void UpdateSoundVolumes()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = PlayerPrefs.GetFloat("Backgroundprefs")*0.005f;
            if (s.name == "Magenta")
            {
                s.source.volume = s.source.volume * 0.3f;
            }
        }
        foreach (Sound s in soundEffects)
        {
            s.source.volume = PlayerPrefs.GetFloat("SoundEffectsPrefs")*0.005f;

        }

    }

}
