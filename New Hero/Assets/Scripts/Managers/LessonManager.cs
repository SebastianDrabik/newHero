using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class LessonManager : MonoBehaviour
{
    public DoorController classExit;
    private GameManager manager;
    public GameObject lessonTriggers;

    public AudioManager AudioManager;
    public GameObject CodeEditor;
    public FightManager attack;

    public GameObject subtitleText;
    public PlayerMovement movement;
    public PauseMenu pauseMenu;

    public SaveData.Level alterDialogueLevel;

    public NPCManager teacher;

    public string examPassTrophyKey;
    public string examKey;

    public string alterDialogueKey;

    public SaveData.Level examPassLevel;

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
        lessonTriggers.SetActive(false);
        if (SaveSystem.level >= alterDialogueLevel)
        {
            UseAlterDialogue();
            teacher.OnDialogueEnd.RemoveAllListeners();
            return;
        }
        manager.ShowObjective("teacher");
        lessonTriggers.SetActive(false);
        classExit.locked = true;
    }

    public void EnableSeats()
    {
        lessonTriggers.SetActive(true);
    }

    public void UseAlterDialogue()
    {
        teacher.dialogueKey = alterDialogueKey;
        teacher.eventEnabled = false;
        teacher.assignObjective = false;
    }

    public void StartLesson()
    {
        CodeEditor.SetActive(true);
        subtitleText.SetActive(true);
        StartCoroutine(nameof(ShowLesson), lessonParts);
        pauseMenu.SetDisabled(true);
    }

    //private Part p;
    //private int currentPart = 0;
    //private int currentPart_subs = 0;
    //private int currentSub = 0;
    //private bool lessonBegan = false;

    //private bool dub = false;

    //private void BeginLesson()
    //{
    //    movement.SetMovementDisabled(true);
    //    lessonBegan = true;

    //    p = lessonParts[currentPart];
    //    currentPart_subs = p.subtitles.Length;
    //    CodeEditor.GetComponent<LessonCodeEditor>().Change(p.code);

    //    subtitleText.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation("lessons", p.subtitles[0]);
    //}

    //public void Previous()
    //{
    //}

    //public void Next()
    //{
    //    if(currentSub == currentPart_subs)
    //    {
    //        if(currentPart == lessonParts.Count)
    //        {
    //            //lesson end
    //            CodeEditor.SetActive(false);
    //            subtitleText.SetActive(false);
    //            movement.SetMovementDisabled(false);
    //            attack.OpenCodeEditor(examKey);
    //        }
    //        else
    //        {
    //            currentPart++;
    //            p = lessonParts[currentPart];
    //            currentSub = 0;

    //            p = lessonParts[currentPart];
    //            currentPart_subs = p.subtitles.Length;
    //            CodeEditor.GetComponent<LessonCodeEditor>().Change(p.code);
    //        }
    //    }
    //    else
    //    {
    //        currentSub++;
    //        subtitleText.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation("lessons", p.subtitles[currentSub]);
    //    }
    //}

    IEnumerator ShowLesson(List<Part> lessonParts)
    {
        movement.SetMovementDisabled(true);
        for (int j = 0; j < lessonParts.Count; j++)
        {
            Part part = lessonParts[j];
            CodeEditor.GetComponent<LessonCodeEditor>().Change(part.code);
            for (int i = 0; i < part.subtitles.Length; i++)
            {
                subtitleText.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation("lessons", part.subtitles[i]);
                if (part.dubs[i] != string.Empty)
                    AudioManager.PlayEffect(part.dubs[i], 0.1f);
                yield return new WaitForSeconds(AudioManager.GetAudioLength(part.dubs[i]));
                if (i < part.subtitles.Length - 1)
                    yield return new WaitUntil(() => Input.anyKeyDown);
            }
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        //lesson end
        CodeEditor.SetActive(false);
        subtitleText.SetActive(false);
        movement.SetMovementDisabled(false);
        attack.OpenCodeEditor(examKey);
    }

    public void HandleCodeExecution(bool result)
    {
        if (result)
        {
            // test passed
            UseAlterDialogue();
            attack.CloseCodeEditor();
            lessonTriggers.SetActive(false);
            classExit.locked = false;
            manager.ChangeTrophyState(examPassTrophyKey, Trophy.TrophyState.UNLOCKED, true);
            manager.HideObjective();
            SaveSystem.level = examPassLevel;
            pauseMenu.SetDisabled(false);
            return;
        }
        attack.EnableHint();
    }
}
