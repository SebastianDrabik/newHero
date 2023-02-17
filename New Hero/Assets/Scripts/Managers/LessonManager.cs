using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class LessonManager : MonoBehaviour
{
    public GameObject classExit;
    private GameManager manager;
    public GameObject lessonTriggers;

    public AudioManager AudioManager;
    public GameObject CodeEditor;
    public FightManager attack;

    public GameObject subtitleText;
    public PlayerMovement movement;

    [System.Serializable]
    public struct Part
    {
        [TextArea(3,30)]
        public string code;
        public string[] subtitles;
        public string[] dubs;
    }

    public List<Part> lessonParts;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (SaveSystem.level >= SaveData.Level.CPP_BASICS)
        {
            //gameObject.SetActive(false);
            return;
        }
        manager.ShowObjective("teacher");
        lessonTriggers.SetActive(false);
        classExit.SetActive(false);
    }

    public void EnableSeats()
    {
        lessonTriggers.SetActive(true);
    }

    public void StartLesson()
    {
        CodeEditor.SetActive(true);
        subtitleText.SetActive(true);
        StartCoroutine(nameof(ShowLesson), lessonParts);
    }


    IEnumerator ShowLesson(List<Part> lessonParts)
    {
        movement.SetMovementDisabled(true);
        for(int j = 0; j < lessonParts.Count; j++)
        {
            Part part = lessonParts[j];
            CodeEditor.GetComponent<LessonCodeEditor>().Change(part.code);
            for (int i = 0; i < part.subtitles.Length; i++)
            {
                subtitleText.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation("first-lesson", part.subtitles[i]);
                if(part.dubs[i] != string.Empty)
                    AudioManager.PlayEffect(part.dubs[i], 0.1f);
                yield return new WaitForSeconds(AudioManager.GetAudioLength(part.dubs[i]));
                if(i<part.subtitles.Length-1)
                    yield return new WaitUntil(() => Input.anyKeyDown);
            }
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        //lesson end
        CodeEditor.SetActive(false);
        subtitleText.SetActive(false);
        movement.SetMovementDisabled(false);
        attack.OpenCodeEditor("test");
    }

    public void HandleCodeExecution(bool result)
    {
        if (result)
        {
            // test passed
            attack.CloseCodeEditor();
            lessonTriggers.SetActive(false);
            classExit.SetActive(true);
            manager.ChangeTrophyState("hello", Trophy.TrophyState.UNLOCKED, true);
            manager.HideObjective();
            SaveSystem.level = SaveData.Level.CPP_BASICS;
            return;
        }
    }
}
