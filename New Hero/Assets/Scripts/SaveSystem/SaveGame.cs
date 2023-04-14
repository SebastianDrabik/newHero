using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{
    private static string currentName = "";

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
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        
        Debug.Log("Save data successfully loaded.");
    }

    private static void SceneManager_sceneLoaded(Scene curren, LoadSceneMode mode)
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if (
            !SaveSystem.tookDamage 
            && gameManager.GetTrophyState("god-run") == Trophy.TrophyState.LOCKED 
            && SaveSystem.level >= SaveData.Level.END_GAME
           )
        {
            gameManager.ChangeTrophyState("god-run", Trophy.TrophyState.UNLOCKED, true);
        }
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    public static void RestartGame()
    {
        StartGame(currentName);
    }
}
