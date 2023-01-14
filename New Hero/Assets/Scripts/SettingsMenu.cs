using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider slider;
    public TMPro.TMP_Dropdown resolutionDropdown;

    public Toggle toggle;

    Resolution[] resolutions;
    private void Start()
    {
        //resolutions

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new();
        var uniqueResolutions = RemoveDuplicates(resolutions);

        int currentResolutionIndex = 0;

        for(int i = 0; i < uniqueResolutions.Length; i++)
        {
            string option = uniqueResolutions[i].width + " x " + uniqueResolutions[i].height;
            options.Add(option);

            if (uniqueResolutions[i].width == Screen.currentResolution.width && uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //slider

        float volume; 
        audioMixer.GetFloat("volume",out volume);

        slider.value = volume;

        //toggle
        toggle.isOn = PlayerPrefs.GetInt("fullscreen") == 1;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    private Resolution[] RemoveDuplicates(Resolution[] res)
    {
        List<Resolution> unique = new();
        List<Resolution> resolutions = new();
        for(int i = 0; i < res.Length; i++)
        {
            resolutions.Add(res[i]);
        }
        foreach(Resolution resolution in res)
        {
            if(!unique.Contains(resolution))
                unique.Add(resolution);
        }
        return unique.ToArray();
    }
}
