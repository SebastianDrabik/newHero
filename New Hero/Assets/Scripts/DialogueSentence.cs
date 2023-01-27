using System;
using UnityEngine;

[Serializable]
public class DialogueSentence
{
    [Header("Name")]
    public string name;
    [TextArea(3,10)]
    public string sentence;
}
