using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveGame : MonoBehaviour
{
    public static void StartGame(string name)
    {
        SaveData save = SaveSystem.LoadData(name);
        GameObject player = GameObject.FindWithTag("Player");
        if (save == null || save.currentScene == null)
        {
            SceneManager.LoadScene("Demo");
            PlayerPrefs.SetFloat("Position_x", 0f);
            PlayerPrefs.SetFloat("Position_y", 0f);

            return;
        }
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        SceneManager.LoadScene(save.currentScene);
        PlayerPrefs.SetFloat("Position_x", save.position[0]);
        PlayerPrefs.SetFloat("Position_y", save.position[1]);
        
        foreach(var t in save.GetTrophies())
        {
            gameManager.ChangeTrophyState(t.Key, t.Value);
        }

        Debug.Log("Save data successfully loaded.");
    }

    public static void RestartGame()
    {
        Debug.Log("F, noob");
    }
}
