using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject achievementNotification;
    [HideInInspector]
    public List<Trophy> trophies;

    private ObjectiveController _ObjectiveController;
    //private MessageController _MessageController;
    private string currentObjective = "";
    private bool isObjectiveShown = false;
    private bool trophiesLoaded = false;
    
    private void Awake()
    {
        EditorTheme.ReadFiles();
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
            Destroy(this);
        if(!trophiesLoaded)
            LoadTrophies();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        AssignController();
    }

    void OnSceneLoaded(Scene current, LoadSceneMode mode)
    {
        AssignController();
        if(trophies.Count == 0)
            LoadTrophies();
        if (isObjectiveShown)
            ShowObjective(currentObjective);
    }

    public void LoadTrophies()
    {
        if (trophiesLoaded)
            return;
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
        trophiesLoaded = true;
    }

    public void ShowObjective(string key)
    {
        if (!isObjectiveShown)
        {
            currentObjective = key;
            isObjectiveShown = true;
        }
        _ObjectiveController.ShowObjective(key);
    }

    public void HideObjective()
    {
        currentObjective=null;
        isObjectiveShown=false;
        _ObjectiveController.HideObjective();
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

    private void AssignController()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            return;
        ObjectiveController oc = GameObject.FindGameObjectWithTag("Canvas").GetComponent<ObjectiveController>();
        if (oc != null)
            _ObjectiveController = oc;
    }
}
