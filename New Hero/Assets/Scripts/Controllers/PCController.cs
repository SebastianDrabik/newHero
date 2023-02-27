using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : MonoBehaviour
{
    public float screwSpeedMult;
    public float timer = 1f;
    public GameObject[] screws;
    public BoxCollider2D trigger;

    private bool unscrew=false;
    private int screwIndex = 0;


    private void Update()
    {
        if (!unscrew) return;

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
        }
    }

    public void HandleEndgame()
    {
        unscrew = true;
    }
}
