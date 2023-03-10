using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackgroundImageController : MonoBehaviour
{
    public float changeTime;
    private readonly List<Sprite> images = new();
    public Image backgroundImage;
    public Image panel;
    public float transitionTime;
    
    private float timer;
    private int currentImageIndex;

    void Awake()
    {
        var resourcesImageList = Resources.LoadAll<Sprite>("MainMenu_Bg");
        foreach (var resource in resourcesImageList)
            images.Add(resource);
        timer = changeTime;     
    }

    void Update()
    {
        if(timer <= 0f)
        {
            timer = changeTime;
            StartCoroutine(nameof(Transition));
        }
        timer-=Time.deltaTime;
    }


    private IEnumerator Transition()
    {
        panel.gameObject.SetActive(true);
        for (int i = 0; i < 100; i++)
        {
            Color color = panel.color;
            color.a = (float)i / 100f;
            panel.color = color;
            yield return new WaitForSeconds(transitionTime/100);
        }
        
        currentImageIndex++;
        if (currentImageIndex >= images.Count)
            currentImageIndex = 0;
        backgroundImage.sprite = images[currentImageIndex];

        for (int i = 100; i >= 0; i--)
        {
            Color color = panel.color;
            color.a = (float)i / 100f;
            panel.color = color;
            yield return new WaitForSeconds(transitionTime/100);
        }
    }

}
