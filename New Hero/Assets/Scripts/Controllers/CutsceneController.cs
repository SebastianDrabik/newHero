using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CutsceneController : MonoBehaviour
{
    public VideoPlayer vid;
    public string SceneName;
    public float endDelay = 0f;
    public Image[] panel;
    public bool start = false;
    void Start()
    {
        StartCoroutine(nameof(AnimationHandle));
    }

    IEnumerator AnimationHandle()
    {
        if (!start)
        {
            yield return new WaitForSeconds((float)vid.length + endDelay - 2f);
            if (panel[0] != null)
            {
                for (int i = 0; i < 100; i++)
                {
                    Color color = panel[0].color;
                    color.a += (float)i / 100;
                    panel[0].color = color;
                    yield return new WaitForSeconds(0.02f);
                }
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
        }
        else
        {
            if (panel[0] != null)
            {
                for (int i = 0; i < 100; i++)
                {
                    Color color = panel[0].color;
                    color.a -= (float)i / 100;
                    panel[0].color = color;
                    yield return new WaitForSeconds(0.02f);
                }
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds((float)vid.length + endDelay - 2f);
        }
        
        SceneManager.LoadScene(SceneName);
        PlayerPrefs.SetFloat("Position_x", 9.45f);
        PlayerPrefs.SetFloat("Position_y", 0f);
    }
}
