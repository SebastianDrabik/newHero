using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class CutsceneController : MonoBehaviour
{
    public VideoPlayer vid;
    public string SceneName;
    public float endDelay = 0f;
    void Start()
    {
        StartCoroutine(nameof(AnimationHandle));
    }

    IEnumerator AnimationHandle()
    {
        yield return new WaitForSeconds((float)vid.length+endDelay);
        SceneManager.LoadScene(SceneName);
        PlayerPrefs.SetFloat("Position_x", 9.45f);
        PlayerPrefs.SetFloat("Position_y", 0f);
    }


}
