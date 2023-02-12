using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject achievementNotification;
    [HideInInspector]
    public List<Trophy> trophies;

    private void Awake()
    {
        EditorTheme.ReadFiles();
        if(Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
            Destroy(this);
    }

    public void LoadTrophies()
    {
        TrophyList trophyList = TranslationsManager.GetTrophies();
        trophies.Clear();
        foreach (var item in trophyList.trophies)
        {
            Trophy nt = new();
            nt.state = item.state;
            nt.key = item.key;
            nt.icon = item.icon;
            nt.title = item.title;
            nt.objective = item.objective;
            trophies.Add(nt);
        }
    }

    public void ChangeTrophyState(string key, Trophy.TrophyState trophyState, bool notification = false)
    {
        trophies.Find(t => t.key == key).state = trophyState;
        if(trophyState == Trophy.TrophyState.UNLOCKED && notification)
        {
            Trophy t = trophies.Find(t => t.key == key);
            Transform canvasTransform = GameObject.FindGameObjectWithTag("Canvas").transform;
            GameObject popup = Instantiate(achievementNotification, canvasTransform);
            popup.GetComponent<TrophyUnlockedNotification>().Show(t);
            GameObject.FindGameObjectWithTag("Audio_Manager").GetComponent<AudioManager>().PlayEffect("Trophy_Collected");
        }
    }

    public Trophy.TrophyState GetTrophyState(string key)
    {
        return trophies.Find(t => t.key == key).state;
    }
}
