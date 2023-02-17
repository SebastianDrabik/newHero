using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject savingInfo;
    public GameObject OSMenu;
    public GameObject pauseMenuUI;
    public GameObject trophies;
    public MessageController messageController;
    //public ObjectiveController objectiveController;

    public static bool GameIsPaused = false;
    private bool isDisabled = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isDisabled) return;
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
        if (trophies.activeSelf)
            trophies.GetComponent<TrophiesManager>().Hide();
        else
        {
            if(OSMenu.activeSelf)
                HideOSMenu();
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            DiscordManager.Instance.SetPlaying(DiscordManager.State.PLAYING);
            GameIsPaused = false;
        }
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

    public void SetDisabled(bool disabled)
    {
        isDisabled = disabled;
    }

    public void ResetBoss()
    {
        SaveSystem.level = SaveData.Level.NEW_GAME;
        GameManager.Instance.ChangeTrophyState("marco", Trophy.TrophyState.LOCKED);
    }
    IEnumerator ShowSavingInfo()
    {
        savingInfo.SetActive(true);
        SaveSystem.SaveData();
        yield return new WaitForSecondsRealtime(2);
        savingInfo.SetActive(false);
    }

    public void ShowOSMenu()
    {
        OSMenu.SetActive(true);
    }

    public void HideOSMenu()
    {
        OSMenu.SetActive(false);
    }

    public void SwitchOSMenu()
    {
        OSMenu.SetActive(!OSMenu.activeSelf);
    }
}
