using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class LessonManager : MonoBehaviour
{
    public GameObject classExit;
    private GameManager manager;

    public AudioManager AudioManager;
    public GameObject CodeEditor;

    public GameObject subtitleText;

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
        manager.ShowObjective("lesson-seat");
        classExit.SetActive(false);
    }

    public void StartLesson()
    {
        CodeEditor.SetActive(true);
        subtitleText.SetActive(true);
        StartCoroutine("ShowLesson", lessonParts);
    }


    IEnumerator ShowLesson(List<Part> lessonParts)
    {
        for(int j = 0; j < lessonParts.Count; j++)
        {
            Part part = lessonParts[j];
            CodeEditor.GetComponent<LessonCodeEditor>().Change(part.code);
            for (int i = 0; i < part.subtitles.Length; i++)
            {
                subtitleText.GetComponent<TextMeshProUGUI>().text = part.subtitles[i];
                AudioManager.PlayEffect(part.dubs[i], 0.1f);
                yield return new WaitForSeconds(AudioManager.GetAudioLength(part.dubs[i]));
                if(i<part.subtitles.Length-1)
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        }
        CodeEditor.SetActive(false);
        subtitleText.SetActive(false);
    }
}
