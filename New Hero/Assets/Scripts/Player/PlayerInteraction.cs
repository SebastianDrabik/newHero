using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.Events;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject image;
    public TextMeshProUGUI healthUI;
    public static readonly int maxHealth = 31;
    private float[] currentCoords = new float[2];
    private string currentScene = "";
    private bool currentDoorsDisabled = false;
    private UnityEvent onLocked;
    private UnityEvent currentInteractionEvent;

    private bool NPC = false;
    private bool lesson = false;
    private bool otherInteraction = false;

    private GameManager gameManager;
    private bool disableInteraction = false;
    private DeathScreenController deathScreenController;

    public void DamagePlayer(int amount)
    {
        int health = SaveSystem.health;
        health -= amount;
        SaveSystem.health = health;
        if (health <= 0)
        {
            KillPlayer();
            return;
        }
        healthUI.text = Convert.ToString(health, 2).PadLeft(8, '0');
    }

    private void KillPlayer()
    {
        //TODO: Death screen
        deathScreenController.ShowDeathScreen();
    }

    private void Start()
    {
        deathScreenController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<DeathScreenController>();
        gameManager = FindObjectOfType<GameManager>();
        if (SaveSystem.health == 0)
            SaveSystem.health = maxHealth;

        healthUI.text = Convert.ToString(SaveSystem.health, 2).PadLeft(8, '0');
        if(!SaveSystem.startDialogue && SceneManager.GetActiveScene().name == "Demo")
        {
            IEnumerator dialogue = StartDial();
            StartCoroutine(dialogue);
        }
        if (PlayerPrefs.HasKey("Interaction_x") && PlayerPrefs.HasKey("Interaction_y"))
        {
            Vector2 position = new(PlayerPrefs.GetFloat("Interaction_x"), PlayerPrefs.GetFloat("Interaction_y"));
            gameObject.transform.SetPositionAndRotation(position, Quaternion.identity);

            PlayerPrefs.DeleteKey("Interaction_x");
            PlayerPrefs.DeleteKey("Interaction_y");
            return;
        }
        gameObject.transform.SetPositionAndRotation(new Vector2(PlayerPrefs.GetFloat("Position_x"), PlayerPrefs.GetFloat("Position_y")), Quaternion.identity);
        PlayerPrefs.DeleteKey("Position_x");
        PlayerPrefs.DeleteKey("Position_y");
    }

    IEnumerator StartDial()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        DialogueManager.currentNPC = "start";
        DialogueManager.assignObjective = false;
        DialogueManager.eventEnabled = false;
        DialogueManager.Instance.StartDialogue("start");
        SaveSystem.startDialogue = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            NPC = true;
            image.SetActive(true);
            return;
        }
        if(collision.gameObject.CompareTag("Seat"))
        {
            lesson = true;
            image.SetActive(true);
            return;
        }
        if (collision.gameObject.GetComponent<InteractionController>() != null)
        {
            var ic = collision.gameObject.GetComponent<InteractionController>();
            if (!ic.disabled)
            {
                currentInteractionEvent = ic.onInteraction;
                otherInteraction = true;
                image.SetActive(true);
                return;
            }
        }
        if (!collision.gameObject.CompareTag("Door"))
            return;
        if(MarkCube.Instance != null)
            if (MarkCube.Instance.isFighting)
                return;
        image.SetActive(true);
        DoorController doors = collision.gameObject.GetComponent<DoorController>();
        currentScene = doors.sceneName;
        currentCoords = doors.coordinates;
        currentDoorsDisabled = doors.locked;
        onLocked = doors.onLocked;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            NPC = false;
            image.SetActive(false);
            return;
        }
        if (collision.gameObject.CompareTag("Seat"))
        {
            lesson = false;
            image.SetActive(false);
            return;
        }
        if (collision.gameObject.GetComponent<InteractionController>() != null)
        {
            otherInteraction = false;
            image.SetActive(false);
            return;
        }
        if (!collision.gameObject.CompareTag("Door"))
            return;
        image.SetActive(false);
        currentScene = "";
        currentCoords = new float[2];
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E) || disableInteraction)
            return;
        if(currentScene != "" && currentScene != "NPC"){
            if (!currentDoorsDisabled)
            {
                PlayerPrefs.SetFloat("Interaction_x", currentCoords[0]);
                PlayerPrefs.SetFloat("Interaction_y", currentCoords[1]);
                SceneManager.LoadScene(currentScene);
            }else
            {
                onLocked.Invoke();
            }
        }
        if (NPC)
        {
            DialogueManager.Instance.StartDialogue();
            NPC = false;
        }
        if (lesson)
        {
            FindObjectOfType<LessonManager>().StartLesson();
            lesson = false;
        }
        if (otherInteraction)
        {
            currentInteractionEvent.Invoke();
            otherInteraction = false;
        }
    }

    public void SetInteractionDisabled(bool disabled)
    {
        disableInteraction = disabled;
    }

    public void Heal()
    {
        SaveSystem.health = maxHealth;
        healthUI.text = Convert.ToString(maxHealth, 2).PadLeft(8, '0');
    }
}
