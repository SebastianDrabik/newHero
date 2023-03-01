using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject achievementNotification;
    [HideInInspector]
    public List<Trophy> trophies;
    private List<Objective> objectives = new();

    private ObjectiveController _ObjectiveController;
    //private MessageController _MessageController;
    public string currentObjective { get; set; } = "";
    private bool isObjectiveShown = false;
    private bool trophiesLoaded = false;
    private bool objectivesLoaded = false;
    
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
            LoadTrophies(true);

        LoadObjectives();
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
            LoadTrophies(true);
        if(objectives.Count == 0)
            LoadObjectives(true);

        if (isObjectiveShown)
            ShowObjective(currentObjective);
    }

    public void LoadObjectives(bool ignoreLoaded = false)
    {
        if (!objectivesLoaded && !ignoreLoaded)
            return;
        objectives.Clear();
        Objective[] objectivesTemp = Resources.LoadAll<Objective>("Objectives");
        for (int i = 0; i < objectivesTemp.Length; i++)
        {
            Objective obj = objectivesTemp[i];
            objectives.Add(obj);
        }
        objectivesLoaded = true;
    }

    public Objective GetObjectiveByKey(string key)
    {
        return objectives.Find(obj => obj.objectiveKey == key);
    }

    public void LoadTrophies(bool ignoreLoaded = false)
    {
        if (trophiesLoaded && !ignoreLoaded)
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
        
        currentObjective = key;
        isObjectiveShown = true;
        _ObjectiveController.ShowObjective(GetObjectiveByKey(key));
    }

    public void HideOneObjective(string obj)
    {
        if (currentObjective != obj)
            return;
        currentObjective = null;
        isObjectiveShown = false;
        _ObjectiveController.HideObjective();
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
        if (SceneManager.GetActiveScene().name == "MainMenu"|| SceneManager.GetActiveScene().name == "Credits"|| SceneManager.GetActiveScene().name == "CutsceneStart"|| SceneManager.GetActiveScene().name == "CutsceneEnd" || _ObjectiveController!=null)
            return;
        ObjectiveController oc = GameObject.FindGameObjectWithTag("Canvas").GetComponent<ObjectiveController>();
        if (oc != null)
        {
            _ObjectiveController = oc;
            string prevobj = SaveSystem.objective;
            if (prevobj != null && prevobj != "" && prevobj != string.Empty)
                oc.ShowObjective(GetObjectiveByKey(prevobj));
        }
    }
}
