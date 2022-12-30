using UnityEngine;

public class SaveGame : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        SaveData save = SaveSystem.LoadData();
        if(save == null)
        {
            return;
        }
        player.transform.position = new Vector3(save.position[0], save.position[1], save.position[2]);

        Debug.Log("Save data successfully loaded.");
    }

    void OnApplicationQuit()
    {
        SaveSystem.SaveData(1);
    }

    public void SaveData()
    {
        SaveSystem.SaveData(1);
    }
}
