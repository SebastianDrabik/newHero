using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue", fileName = "NewDialogue")]
public class Dialogue : ScriptableObject
{
    [Header("Unique dialogue key")]
    public string key;
    [Space]
    public List<DialogueSentence> sentences;
}
