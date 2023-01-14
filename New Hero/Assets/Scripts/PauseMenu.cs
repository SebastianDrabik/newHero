using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject savingInfo;

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        DiscordManager.Instance.SetPlaying(DiscordManager.State.PLAYING);
        GameIsPaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        DiscordManager.Instance.SetPlaying(DiscordManager.State.PAUSE);
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Resume();
        DiscordManager.Instance.SetPlaying(DiscordManager.State.MENU);
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void SaveGame()
    {
        StartCoroutine(ShowSavingInfo());
    }

    public void ShowTrophies()
    {
        Debug.Log("Showing acheviements...");
    }

    public void ResetBoss()
    {
        PlayerPrefs.SetInt("Marco_Defeated", 0);
    }
    IEnumerator ShowSavingInfo()
    {
        savingInfo.SetActive(true);
        SaveSystem.SaveData(1);
        yield return new WaitForSecondsRealtime(2);
        savingInfo.SetActive(false);
    }
}
