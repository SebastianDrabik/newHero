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
    }

    public List<Part> lessonParts;

    public void StartLesson()
    {
        CodeEditor.SetActive(true);
        subtitleText.SetActive(true);
        StartCoroutine(nameof(ShowLesson), lessonParts);
        pauseMenu.SetDisabled(true);
    }

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
        attack.OpenCodeEditor("sgp-exam");
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
            return;
        }
        attack.EnableHint();
    }
}
