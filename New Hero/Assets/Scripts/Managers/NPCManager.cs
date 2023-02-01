using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public string dialogueKey;

    void OnTriggerEnter2D(Collider2D collision)
    {
        DialogueManager.currentNPC = dialogueKey.ToLower();    
    }
}
