using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject image;
    public TextMeshProUGUI healthUI;
    readonly int maxHealth = 31;
    readonly string[] tags = { "Elektryk", "Elektryk_Exit", "Cube", "Cube_Exit","Cave","Cave_Exit", "NPC" };
    private string currentTag = "";

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
        SaveGame.StartGame();
        SaveSystem.health = maxHealth;
    }

    private void Start()
    {
        if (SaveSystem.health == 0)
            SaveSystem.health = maxHealth;

        healthUI.text = Convert.ToString(SaveSystem.health, 2).PadLeft(8, '0');

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool exists = Array.Exists(tags, element => element == collision.gameObject.tag);
        if (exists)
            if ((collision.gameObject.tag == "Cube_Exit" && !MarkCube.Instance.isFighting) || collision.gameObject.tag != "Cube_Exit")
                image.SetActive(true);
        currentTag = collision.gameObject.tag;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        bool exists = Array.Exists(tags, element => element == collision.gameObject.tag);
        if (exists)
            image.SetActive(false);
        currentTag = "";
    }

    void Update()
    {
        if (currentTag == "Elektryk" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Entering Elektryk");
            PlayerPrefs.SetFloat("Interaction_x", 0f);
            PlayerPrefs.SetFloat("Interaction_y", 0f);

            SceneManager.LoadScene("Elektryk");
        }
        if (currentTag == "Elektryk_Exit" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Exiting Elektryk");
            PlayerPrefs.SetFloat("Interaction_x", 16f);
            PlayerPrefs.SetFloat("Interaction_y", -18f);

            SceneManager.LoadScene("Demo");
        }
        if (currentTag == "Cube" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Entering Cube Office");
            PlayerPrefs.SetFloat("Interaction_x", -0.5f);
            PlayerPrefs.SetFloat("Interaction_y", 0f);

            SceneManager.LoadScene("MarkCube_Boss");
        }
        if (currentTag == "Cube_Exit" && Input.GetKeyDown(KeyCode.E) && !MarkCube.Instance.isFighting)
        {
            Debug.Log("Exiting Cube Office");
            PlayerPrefs.SetFloat("Interaction_x", -45.5f);
            PlayerPrefs.SetFloat("Interaction_y", 23.5f);

            SceneManager.LoadScene("Elektryk");
        }

        if (currentTag == "Cave" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Entering Cave");
            PlayerPrefs.SetFloat("Interaction_x", -0.5f);
            PlayerPrefs.SetFloat("Interaction_y", 0f);

            SceneManager.LoadScene("Boss_Cave");
        }
        if (currentTag == "Cave_Exit" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Exiting Cave");
            PlayerPrefs.SetFloat("Interaction_x", 139f);
            PlayerPrefs.SetFloat("Interaction_y", 25f);

            SceneManager.LoadScene("Demo");
        }

        if (currentTag == "NPC" && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.Instance.StartDialogue("nft_trader");
        }
    }
}
