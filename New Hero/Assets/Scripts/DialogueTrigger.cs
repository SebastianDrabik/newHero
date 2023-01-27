using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public void TriggerDialogue(int index)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(index);
    }
}