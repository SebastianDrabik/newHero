using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicController : MonoBehaviour
{
    public BoxCollider2D musicSpace;
    private AudioManager audioM;
    public string musicKey;

    private void Start()
    {
        audioM = GameObject.FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(nameof(delay));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioM.StopAll();
            audioM.Play("MainTheme");
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForEndOfFrame();
        audioM = GameObject.FindObjectOfType<AudioManager>();
        audioM.StopAll();
        audioM.Play(musicKey);
    }
}
