using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PythonBattleStarter : MonoBehaviour
{
    public GameObject python;
    public CinemachineVirtualCamera cam;

    public PlayerMovement movement;

    public GameObject healthbar;
    public GameObject audioSpace;

    public SaveData.Level minLevel;

    void Start()
    {
        if (SaveSystem.level >= SaveData.Level.PYTHON)
        {
            audioSpace.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SaveSystem.level < minLevel)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            movement.SetMovementDisabled(true);
            healthbar.SetActive(true);
            StartCoroutine(nameof(AnimatePython));
        }
    }

    IEnumerator AnimatePython()
    {
        cam.Follow = python.transform;
        python.GetComponent<Animator>().SetTrigger("Change");
        yield return new WaitForSeconds(7.0f);
        cam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        python.GetComponent<Python>().fightStarted = true;
        gameObject.SetActive(false);
        movement.SetMovementDisabled(false);
    }
}
