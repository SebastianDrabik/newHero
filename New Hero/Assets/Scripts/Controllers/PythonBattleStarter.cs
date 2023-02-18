using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PythonBattleStarter : MonoBehaviour
{
    public GameObject python;
    public CinemachineVirtualCamera cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine("AnimatePython");
        }
    }

    IEnumerator AnimatePython()
    {
        cam.Follow = python.transform;
        python.GetComponent<Animator>().SetTrigger("Change");
        yield return new WaitForSeconds(7.0f);
        cam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.SetActive(false);
    }
}
