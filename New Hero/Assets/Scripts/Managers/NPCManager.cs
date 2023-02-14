using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public string dialogueKey;
    [Space]
    public bool assignObjective;
    public string objectiveDescription;

    void OnTriggerEnter2D(Collider2D collision)
    {
        DialogueManager.currentNPC = dialogueKey.ToLower();  
        DialogueManager.objectiveDescription = objectiveDescription;
        DialogueManager.assignObjective = assignObjective;
    }
}
