using System;
using TMPro;
using UnityEngine;

public class TranslationController : MonoBehaviour
{
    public string group;
    public string key;
    public bool watchEnable = false;

    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation(group, key);
    }

    public void UpdateContent()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation(group, key);
    }

    void OnEnable()
    {
        if(watchEnable)
            gameObject.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation(group, key);
    }
}
