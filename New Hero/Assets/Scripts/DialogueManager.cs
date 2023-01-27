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
    public PauseMenu pauseMenu;
    public GameObject infoKey;

    private Queue<DialogueSentence> sentences;

    private PlayerMovement playerMovement;

    private bool isTalking = false;
    private bool isTyping = false;

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
        if (isTalking) return;
        isTalking = true;
        infoKey.SetActive(false);

        playerMovement.SetMovementDisabled(true);
        pauseMenu.SetDisabled(true);

        Dialogue dialogue = DialogueList.Find(d => d.key == key);
        if(dialogue == null)
        {
            Debug.LogWarning("Cannot find dialog with key: " + key);
            return;
        }

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
        if(isTyping) return;

        var sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence.name, sentence.sentence));
    }

    IEnumerator TypeSentence(string name, string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        nameText.text = name;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        isTalking = false;
        playerMovement.SetMovementDisabled(false);
        pauseMenu.SetDisabled(false);
        animator.SetBool("IsOpen", false);
    }
}
