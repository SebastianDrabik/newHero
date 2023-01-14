using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
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
        if (PlayerPrefs.HasKey("Marco_Defeated") && SceneManager.GetActiveScene().name == "MarkCube_Boss")
        {
            if (PlayerPrefs.GetInt("Marco_Defeated") == 1)
            {
                StopAll();
                Play("MainTheme");
            }else if(PlayerPrefs.GetInt("Marco_Defeated") == 0)
            {
                StopAll();
                Play("Marco_Music_Loop");
            }
        }
        else if (currentScene != "MarkCube_Boss")
        {
            Play("MainTheme");
        }
        else
        {
            Play("Marco_Music_Loop");
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != currentScene)
        {
            if(SceneManager.GetActiveScene().name == "MarkCube_Boss")
            {
                StopAll();
                Play("Marco_Music_Loop");
            }
            else if(currentScene == "MarkCube_Boss")
            {
                StopAll();
                Play("MainTheme");
            }
            currentScene = SceneManager.GetActiveScene().name;
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
