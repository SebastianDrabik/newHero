using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static SaveData.Level level;
    public static int health;
    public static string path;
    public static string objective;
    public static bool startDialogue = false;
    public static bool tookDamage = false;

    public static void SaveData()
    {
        if(path == null)
        {
            Debug.LogWarning("Cannot save - no save is loaded.");
            return;
        }
        // position
        Vector3 position = GameObject.FindWithTag("Player").transform.position;
        // trophies
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        BinaryFormatter formatter = new();
        FileStream fs = new(path, FileMode.Create);
        SaveData save = new(level, position, SceneManager.GetActiveScene().name, ConvertTrophies(gameManager.trophies), health, gameManager.currentObjective, startDialogue, tookDamage);
        formatter.Serialize(fs, save);
        fs.Close();
        Debug.Log("Successfully saved game");
    }

    public static SaveData LoadData(string savePath)
    {
        path = savePath;
        if (!File.Exists(path))
        {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
        FileStream fs = new(path, FileMode.Open);
        BinaryFormatter formatter = new();
        SaveData save = formatter.Deserialize(fs) as SaveData;
        if(save == null)
        {
            Debug.LogWarning("Save data is empty");
            level = global::SaveData.Level.NEW_GAME;
            health = 31;
            return null;
        }
        level = save.level;
        health = save.health;
        objective = save.currentObjective;
        startDialogue = save.startDialogue;
        tookDamage = save.tookDamage;
        fs.Close();

        return save;
    }

    private static Dictionary<string, Trophy.TrophyState> ConvertTrophies(List<Trophy> toConvert)
    {
        Dictionary<string, Trophy.TrophyState> finalDict = new();
        foreach (var toConvertItem in toConvert)
        {
            finalDict.Add(toConvertItem.key, toConvertItem.state);
        }
        return finalDict;
    }
}
