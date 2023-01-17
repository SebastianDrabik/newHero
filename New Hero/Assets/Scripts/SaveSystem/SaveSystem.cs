using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static SaveData.Level level;
    private static string fileName = "saveData.dat";

    public static void SaveData()
    {
        // position
        Vector3 position = GameObject.FindWithTag("Player").transform.position;
        // trophies
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName;
        FileStream fs = new FileStream(path, FileMode.Create);
        SaveData save = new SaveData(level, position, SceneManager.GetActiveScene().name, ConvertTrophies(gameManager.trophies));
        formatter.Serialize(fs, save);
        fs.Close();
        Debug.Log("Successfully saved game");
    }

    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if(!File.Exists(path))
        {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }

        FileStream fs = new FileStream(path, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        SaveData save = formatter.Deserialize(fs) as SaveData;
        level = save.level;
        fs.Close();
        return save;
    }

    private static Dictionary<string, Trophy.TrophyState> ConvertTrophies(List<Trophy> toConvert)
    {
        Dictionary<string, Trophy.TrophyState> finalDict = new();
        foreach(var toConvertItem in toConvert)
        {
            finalDict.Add(toConvertItem.key, toConvertItem.state);
        }
        return finalDict;
    }
}
