using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int level;
    public float[] position;
    public string currentScene;

    public SaveData(int level, Vector3 position, string currentScene)
    {
        this.level = level;

        this.position = new float[3];
        this.position[0] = position.x;
        this.position[1] = position.y;
        this.position[2] = position.z;

        this.currentScene = currentScene;
    }
}
