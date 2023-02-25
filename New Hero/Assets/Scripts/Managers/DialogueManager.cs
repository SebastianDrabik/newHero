using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static string currentNPC;

    public static DialogueManager Instance;
    public List<Dialogue> DialogueList;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;
    public PauseMenu pauseMenu;
    public GameObject infoKey;
    public float dialogueSpeed;

    public static bool assignObjective;
    public static string objectiveDescription;
    public static UnityEvent OnDialogueEnd;
    public static bool eventEnabled;

    private Queue<DialogueSentence> sentences;
    private PlayerMovement playerMovement;

    private GameManager manager;

    private bool isTalking = false;
    private bool isTyping = false;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        LoadAllDialogues(TranslationsManager.lang);
    }

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        sentences = new Queue<DialogueSentence>();
    }

    void Update()
    {
        if(Input.anyKey && isTalking)
            DisplayNextSentence();
    }

    public void StartDialogue(string key = "")
    {
        if (isTalking) return;
        isTalking = true;
        infoKey.SetActive(false);

        playerMovement.SetMovementDisabled(true);
        pauseMenu.SetDisabled(true);

        Dialogue dialogue;
        if(key.Length > 0)
            dialogue = DialogueList.Find(d => d.key.ToLower() == key.ToLower());
        else
            dialogue = DialogueList.Find(d => d.key.ToLower() == currentNPC);
        if (dialogue == null)
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
        if(isTyping) return;
        if (sentences.Count == 0)
        {
            AssignObjective();
            EndDialogue();
            return;
        }

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
            yield return new WaitForSeconds(dialogueSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        isTalking = false;
        playerMovement.SetMovementDisabled(false);
        pauseMenu.SetDisabled(false);
        animator.SetBool("IsOpen", false);
        //print("event: " + eventEnabled);
        if(eventEnabled)
            OnDialogueEnd.Invoke();
    }

    private void LoadAllDialogues(string lang)
    {
        Dialogue[] tempDialogue = Resources.LoadAll<Dialogue>($"Dialogues/{lang}");
        foreach (Dialogue dialogue in tempDialogue)
            DialogueList.Add(dialogue);
    }

    private void AssignObjective()
    {
        if (!assignObjective) return;
        manager.ShowObjective(objectiveDescription);

        //if(objectiveDescription == "lesson-seat")
        //    FindObjectOfType<LessonManager>()?.EnableSeats();
    }
}
