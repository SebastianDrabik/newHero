using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string fileName = "data.dat";

    public static void SaveData(int level)
    {
        Vector3 position = GameObject.FindWithTag("Player").transform.position;

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName;
        FileStream fs = new FileStream(path, FileMode.Create);
        SaveData save = new SaveData(level, position);
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
        fs.Close();
        return save;
    }
}
