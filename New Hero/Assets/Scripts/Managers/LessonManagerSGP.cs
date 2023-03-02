using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class LessonManagerSGP : MonoBehaviour
{
    private GameManager manager;

    public GameObject SGP;
    public AudioManager AudioManager;
    public GameObject CodeEditor;
    public FightManager attack;

    public GameObject subtitleText;
    public PlayerMovement movement;
    public PauseMenu pauseMenu;

    public string examPassTrophyKey;

    public SaveData.Level examPassLevel;

    [System.Serializable]
    public struct Part
    {
        [TextArea(3, 30)]
        public string code;
        public string[] subtitles;
        public string[] dubs;
    }

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if(SaveSystem.level >= SaveData.Level.CPP_MASTER)
        {
            SGP.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!lessonBegan)
            return;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Previous();
        else if(Input.GetKeyDown(KeyCode.RightArrow))
            Next();
    }

    public List<Part> lessonParts;

    public void StartLesson()
    {
        CodeEditor.SetActive(true);
        subtitleText.SetActive(true);
        BeginLesson();
        pauseMenu.SetDisabled(true);
    }

    private Part p;
    private int currentPart = 0;
    private int currentPart_subs = 0;
    private int currentSub = 0; //index
    private bool lessonBegan = false;
    private bool lessonEnded = false;

    private bool dub = false;


    private void BeginLesson()
    {
        lessonBegan = true;
        movement.SetMovementDisabled(true);
        lessonBegan = true;

        p = lessonParts[currentPart];
        currentPart_subs = p.subtitles.Length;
        CodeEditor.GetComponent<LessonCodeEditor>().Change(p.code);

        subtitleText.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation("lessons", p.subtitles[0]);
        if (p.dubs[currentSub] != string.Empty && p.dubs[currentSub] != null)
        {
            IEnumerator sound = Sound(p.dubs[currentSub]);
            StartCoroutine(sound);
        }
    }


    public void Previous()
    {
        if (dub || attack.gameObject.activeSelf)
            return;
        if (lessonBegan && currentPart >= 0 && currentSub > 0)
        {
            currentSub--;
            subtitleText.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation("lessons", p.subtitles[currentSub]);
            if (p.dubs[currentSub] != string.Empty)
            {
                IEnumerator sound = Sound(p.dubs[currentSub]);
                StartCoroutine(sound);
            }
        }
        else if (lessonBegan && currentPart > 0 && currentSub == 0)
        {
            currentPart--;
            p = lessonParts[currentPart];
            currentPart_subs = p.subtitles.Length;
            currentSub = currentPart_subs - 1;
            subtitleText.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation("lessons", p.subtitles[currentSub]);
            if (p.dubs[currentSub] != string.Empty)
            {
                IEnumerator sound = Sound(p.dubs[currentSub]);
                StartCoroutine(sound);
            }
            CodeEditor.GetComponent<LessonCodeEditor>().Change(p.code);
        }

        if (lessonEnded)
            lessonEnded = false;
    }

    public void Next()
    {
        if (dub || attack.gameObject.activeSelf)
            return;
        if (lessonEnded)
        {
            CodeEditor.SetActive(false);
            subtitleText.SetActive(false);
            movement.SetMovementDisabled(false);
            attack.OpenCodeEditor("sgp-exam");
            lessonEnded = false;
            currentPart = 0;
            currentSub = 0;
            p = lessonParts[currentPart];
            currentPart_subs = p.subtitles.Length;
            return;
        }
        if (lessonBegan && currentPart <= lessonParts.Count - 1 && currentSub < currentPart_subs - 1)
        {
            currentSub++;
            subtitleText.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation("lessons", p.subtitles[currentSub]);
            if (p.dubs[currentSub] != string.Empty)
            {
                IEnumerator sound = Sound(p.dubs[currentSub]);
                StartCoroutine(sound);
            }
        }
        else if (lessonBegan && currentPart < lessonParts.Count - 1)
        {
            currentPart++;
            p = lessonParts[currentPart];
            currentPart_subs = p.subtitles.Length;
            currentSub = 0;
            subtitleText.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation("lessons", p.subtitles[currentSub]);
            if (p.dubs[currentSub] != string.Empty)
            {
                IEnumerator sound = Sound(p.dubs[currentSub]);
                StartCoroutine(sound);
            }
            CodeEditor.GetComponent<LessonCodeEditor>().Change(p.code);
        }
        if (currentPart == lessonParts.Count - 1 && p.subtitles.Length - 1 == currentSub)
        {
            lessonEnded = true;
        }
    }

    IEnumerator Sound(string soundKey)
    {
        AudioManager AudioManager = GameObject.FindObjectOfType<AudioManager>();
        AudioManager.PlayEffect(soundKey, 0.05f);
        float length = AudioManager.GetAudioLength(soundKey);
        dub = true;
        if (Application.isEditor)
            length = 0f;
        yield return new WaitForSecondsRealtime(length);
        dub = false;
    }

    public void HandleCodeExecution(bool result)
    {
        if (result)
        {
            // test passed
            attack.CloseCodeEditor();
            manager.ChangeTrophyState(examPassTrophyKey, Trophy.TrophyState.UNLOCKED, true);
            manager.HideObjective();
            SaveSystem.level = examPassLevel;
            pauseMenu.SetDisabled(false);
            manager.HideObjective();
            manager.ChangeTrophyState(examPassTrophyKey, Trophy.TrophyState.UNLOCKED, true);
            SGP.SetActive(false);
            gameObject.SetActive(false);
            return;
        }
        attack.EnableHint();
    }
}
