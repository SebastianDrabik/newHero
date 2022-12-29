using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer mixer;

    void Start()
    {
        loadOptions();
    }

    void OnApplicationQuit()
    {
        saveOptions();
    }

    public void saveOptions()
    {
        float volume;
        mixer.GetFloat("volume", out volume);
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
        PlayerPrefs.SetInt("resolution_width", Screen.currentResolution.width);
        PlayerPrefs.SetInt("resolution_height", Screen.currentResolution.height);
    }

    public void loadOptions()
    {
        Screen.fullScreen = PlayerPrefs.GetInt("fullscreen") == 1;

        Screen.SetResolution(PlayerPrefs.GetInt("resolution_width"), PlayerPrefs.GetInt("resolution_height"), PlayerPrefs.GetInt("fullscreen") == 1);

        mixer.SetFloat("volume", PlayerPrefs.GetFloat("volume"));
        
    }

}
