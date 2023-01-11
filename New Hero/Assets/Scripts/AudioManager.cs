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
        if (currentScene != "MarkCube_Boss")
        {
            Play("MainTheme");
        }
        else
        {
            PlayMarcoMusic();
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != currentScene)
        {
            if(SceneManager.GetActiveScene().name == "MarkCube_Boss")
            {
                StopAll();
                PlayMarcoMusic();
            }
            else if(currentScene == "MarkCube_Boss")
            {
                StopAll();
                Play("MainTheme");
            }
            currentScene = SceneManager.GetActiveScene().name;
        }
    }

    public void PlayMarcoMusic()
    {
        StartCoroutine(playMarcoSound());
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

    IEnumerator playMarcoSound()
    {
        Sound s = Array.Find(sounds, sound => sound.name == "Marco_Music_Start");
        if (s == null)
        {
            Debug.LogWarning("Audio: " + "Marco_Music_Start" + " not found");
            yield break;
        }
        s.source.Play();
        Debug.Log(s.source.clip.length);
        yield return new WaitForSeconds(Mathf.Floor(s.source.clip.length));
        if (SceneManager.GetActiveScene().name != "MarkCube_Boss")
        {
            yield break;
        }
        s = Array.Find(sounds, sound => sound.name == "Marco_Music_Loop");
        Debug.Log(s.source.clip.length);
        s.source.Play();
    }
}
