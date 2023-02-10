using System;
using TMPro;
using UnityEngine;

public class TranslationController : MonoBehaviour
{
    public string group;
    public string key;

    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = TranslationsManager.GetTranslation(group, key);
    }
}
