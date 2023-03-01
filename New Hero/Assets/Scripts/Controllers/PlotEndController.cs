using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlotEndController : MonoBehaviour
{
    public float seconds;
    private void Start()
    {
        StartCoroutine(nameof(delay));
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("CutsceneEnd");
    }
}
