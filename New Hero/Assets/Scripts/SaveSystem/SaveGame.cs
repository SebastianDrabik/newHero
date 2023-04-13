using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{
    private static string currentName = "";

    public static UnityAction<Scene, LoadSceneMode> OnSceneLoaded { get; private set; }

    public static void StartGame(string name)
    {
        currentName = name;

        SaveData save = SaveSystem.LoadData(name);
        //GameObject player = GameObject.FindWithTag("Player");
        if (save == null || save.currentScene == null)
        {
            SceneManager.LoadScene("CutsceneStart");
            PlayerPrefs.SetFloat("Position_x", 9.45f);
            PlayerPrefs.SetFloat("Position_y", 0f);

            return;
        }
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        SceneManager.LoadScene(save.currentScene);
        PlayerPrefs.SetFloat("Position_x", save.position[0]);
        PlayerPrefs.SetFloat("Position_y", save.position[1]);

        foreach (var t in save.GetTrophies())
        {
            gameManager.ChangeTrophyState(t.Key, t.Value);
        }
        if (!SaveSystem.tookDamage && gameManager.GetTrophyState("god-run") == Trophy.TrophyState.LOCKED)
        {
            gameManager.ChangeTrophyState("god-run", Trophy.TrophyState.UNLOCKED, true);
        }
        Debug.Log("Save data successfully loaded.");
    }

    public static void RestartGame()
    {
        StartGame(currentName);
    }
}
