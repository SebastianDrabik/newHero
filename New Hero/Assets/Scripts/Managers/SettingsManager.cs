using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer mixer;

    void Start()
    {
        LoadOptions();
    }

    void OnApplicationQuit()
    {
        SaveOptions();
    }

    public void SaveOptions()
    {
        float volume;
        mixer.GetFloat("volume", out volume);
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
        PlayerPrefs.SetInt("resolution_width", Screen.currentResolution.width);
        PlayerPrefs.SetInt("resolution_height", Screen.currentResolution.height);
    }

    public void LoadOptions()
    {
        Screen.fullScreen = PlayerPrefs.GetInt("fullscreen") == 1;

        Screen.SetResolution(PlayerPrefs.GetInt("resolution_width"), PlayerPrefs.GetInt("resolution_height"), PlayerPrefs.GetInt("fullscreen") == 1);

        mixer.SetFloat("volume", PlayerPrefs.GetFloat("volume"));

        if (PlayerPrefs.HasKey("language"))
            TranslationsManager.lang = PlayerPrefs.GetString("language");
        else
            TranslationsManager.lang = TranslationsManager.defaultLang;
        TranslationsManager.UpdateSaveListErrorMessages();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().LoadTrophies(true);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().LoadObjectives();
    }

}
