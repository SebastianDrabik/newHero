using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallController : MonoBehaviour
{
    public FightManager editor;
    public ParticleSystem fire;
    public GameObject face;
    public BoxCollider2D trigger;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCode()
    {
        editor.OpenCodeEditor("firewall");
    }

    public void HandleCodeExecution(bool result)
    {
        if (result)
        {
            // win
            editor.CloseCodeEditor();
            fire.Stop();
            face.SetActive(false);
            StartCoroutine(nameof(TurnOff));
            return;
        }
        editor.CloseCodeEditor();

    }

    IEnumerator TurnOff()
    {
        trigger.enabled = false;
        yield return new WaitForSeconds(fire.startLifetime);
        gameObject.SetActive(false);
    }
}
