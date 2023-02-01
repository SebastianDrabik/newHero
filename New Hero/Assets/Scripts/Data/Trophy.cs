using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trophy
{
    public enum TrophyState
    {
        LOCKED = 0,
        UNLOCKED = 1,
        IN_PROGRESS = 2,
    }
    [Header("Trophy unique key")]
    public string key;
    public TrophyState state;
    public string title;
    public string objective;
    public Sprite icon;
}
