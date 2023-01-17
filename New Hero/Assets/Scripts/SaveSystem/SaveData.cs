using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public enum Level
    {
        NEW_GAME = 0,
        MARK_CUBE = 1,
    }
    public Level level;
    public float[] position;
    public string currentScene;
    readonly Dictionary<string, Trophy.TrophyState> trophies;

    public SaveData(Level level, Vector3 position, string currentScene, Dictionary<string, Trophy.TrophyState> trophies)
    {
        this.level = level;

        this.position = new float[3];
        this.position[0] = position.x;
        this.position[1] = position.y;
        this.position[2] = position.z;

        this.currentScene = currentScene;

        this.trophies = trophies;
    }

    public Dictionary<string, Trophy.TrophyState> GetTrophies()
    {
        return trophies;
    }
}
