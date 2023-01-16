using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
            if(PlayerPrefs.HasKey("Marco_Defeated"))
                if(PlayerPrefs.GetInt("Marco_Defeated") == 1)
                    ChangeTrophyState("marco", Trophy.TrophyState.UNLOCKED);
        }
        else
            Destroy(this);
    }

    public List<Trophy> trophies;

    public void ChangeTrophyState(string key, Trophy.TrophyState trophyState)
    {
        trophies.Find(t => t.key == key).state = trophyState;
    }
}
