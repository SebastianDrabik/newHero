using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveGame : MonoBehaviour
{
    public void StartGame()
    {
        SaveData save = SaveSystem.LoadData();
        GameObject player = GameObject.FindWithTag("Player");
        if (save == null || save.currentScene == null)
        {
            SceneManager.LoadScene("Demo");
            PlayerPrefs.SetFloat("Position_x", 0f);
            PlayerPrefs.SetFloat("Position_y", 0f);

            return;
        }
        SceneManager.LoadScene(save.currentScene);
        PlayerPrefs.SetFloat("Position_x", save.position[0]);
        PlayerPrefs.SetFloat("Position_y", save.position[1]);

        Debug.Log("Save data successfully loaded.");
    }

    public void Save()
    {
        SaveSystem.SaveData(1);
    }
}
