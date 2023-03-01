using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PCController : MonoBehaviour
{
    public float screwSpeedMult;
    public float timer = 1f;
    public GameObject[] screws;
    public BoxCollider2D trigger;
    public PlayerMovement movement;

    public Image panel;


    private bool unscrew=false;
    private int screwIndex = 0;


    private void Update()
    {
        if (!unscrew) return;

        if (!movement.movementDisabled)
        {
            movement.SetMovementDisabled(true);
        }
        if (screwIndex < screws.Length)
        {
            screws[screwIndex].transform.Rotate(new Vector3(0f,0f,-screwSpeedMult*Time.deltaTime));
            timer -= Time.deltaTime;
            if (timer<=0f)
            {
                screws[screwIndex].SetActive(false);
                timer = 2f;
                screwIndex++;
            }
        }
        else
        {
            unscrew = false;
            trigger.enabled = false;
            StartCoroutine(nameof(WaitCutscene));
        }
    }

    public void HandleEndgame()
    {
        unscrew = true;
    }

    IEnumerator WaitCutscene()
    {
        panel.gameObject.SetActive(true);
        for(int i = 0; i < 100; i++)
        {
            Color color = panel.color;
            color.a += (float)i / 100;
            panel.color = color;
            yield return new WaitForSeconds(0.03f);
        }
        SceneManager.LoadScene("CutsceneEnd");
        SaveSystem.level = SaveData.Level.END_GAME;
        SaveSystem.SaveData();
    }
}
