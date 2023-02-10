using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackgroundImageController : MonoBehaviour
{
    public float changeTime;
    private readonly List<Sprite> images = new();
    public Image backgroundImage;
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
            currentImageIndex++;
            if(currentImageIndex >= images.Count)
                currentImageIndex = 0;
            backgroundImage.sprite = images[currentImageIndex];
        }
        timer-=Time.deltaTime;
    }

}
