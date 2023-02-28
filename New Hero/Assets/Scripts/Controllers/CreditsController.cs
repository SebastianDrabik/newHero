using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CreditsController : MonoBehaviour
{
    public RectTransform panel;
    public int speed;
    public float delay = 2f;

    void Update()
    {
        if (delay > 0f)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            //panel.position.Set(panel.position.x, panel.position.y-(Time.deltaTime*speed), panel.position.z);
            panel.offsetMax = new Vector2(panel.offsetMax.x, panel.offsetMax.y - (Time.deltaTime * speed));
            Debug.Log(panel.offsetMax.y);
            if(panel.offsetMax.y >= 5500)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

    }
    
}
