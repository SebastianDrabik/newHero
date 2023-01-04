using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SaveGame savegame;
    public void PlayGame()
    {
        DiscordManager.Instance.SetPlaying(DiscordManager.State.PLAYING);
        savegame.StartGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
