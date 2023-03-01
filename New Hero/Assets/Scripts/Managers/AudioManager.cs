using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    string currentScene;
    public Sound[] sounds;

    public static AudioManager instance;
    void Awake()
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

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.group;
        }
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        if (!IsPlaying("MainTheme"))
        {
            StopAll();
            Play("MainTheme");
            return;
        }
        if(currentScene == "MainMenu")
        {
            StopAll();
            Play("MainTheme");
            return;
        }

    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Audio: " + name + " not found");
            return;
        }
        s.source.Play();
    }

    public float GetAudioLength(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Audio: " + name + " not found");
            return 0f;
        }
        return s.source.clip.length;
    }

    public void PlayEffect(string name, float otherSoundsMaxVolume = 0.2f)
    {
        Dictionary<Sound, float> playingSounds= new();
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound current = sounds[i];
            if (IsPlaying(current.name) && current.loop)
            {
                playingSounds.Add(sounds[i], current.volume);
                current.volume = Math.Clamp(current.volume - 0.2f, 0.01f, otherSoundsMaxVolume);
                current.source.volume = current.volume;
            }
        }
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Audio: " + name + " not found");
            StartCoroutine(RestoreSounds(playingSounds, 0f));
            return;
        }
        s.source.Play();
        StartCoroutine(RestoreSounds(playingSounds, s.source.clip.length));
    }

    IEnumerator RestoreSounds(Dictionary<Sound, float> toRestore, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        foreach(var item in toRestore)
        {
            item.Key.volume = item.Value;
            item.Key.source.volume = item.Key.volume;
        }
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Audio: " + name + " not found");
            return false;
        }
        return s.source.isPlaying;
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Audio: " + name + " not found");
            return;
        }
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (AudioSource s in gameObject.GetComponents<AudioSource>())
        {
            s.Stop();   
        }
    }


}
