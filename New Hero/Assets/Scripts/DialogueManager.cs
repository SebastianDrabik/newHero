using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public List<Dialogue> DialogueList;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;

    private Queue<DialogueSentence> sentences;

    private PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        sentences = new Queue<DialogueSentence>();
    }

    public void StartDialogue(string key)
    {
        playerMovement.SetMovementDisabled(true);

        Dialogue dialogue = DialogueList.Find(d => d.key == key);

        animator.SetBool("IsOpen", true);

        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            nameText.text = sentence.name;
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence.name, sentence.sentence));
    }

    IEnumerator TypeSentence(string name, string sentence)
    {
        dialogueText.text = "";
        nameText.text = name;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        playerMovement.SetMovementDisabled(false);
        animator.SetBool("IsOpen", false);
    }
}
