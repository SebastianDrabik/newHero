using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject image;

    string[] tags = {"Elektryk", "Elektryk_Exit","Cube","Cube_Exit" };

    private string currentTag="";

    private void Start()
    {
        if(PlayerPrefs.HasKey("Interaction_x") && PlayerPrefs.HasKey("Interaction_y"))
        {
            Vector2 position = new Vector2(PlayerPrefs.GetFloat("Interaction_x"), PlayerPrefs.GetFloat("Interaction_y"));
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
        {
            image.SetActive(true);
        }
        currentTag = collision.gameObject.tag;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        bool exists = Array.Exists(tags, element => element == collision.gameObject.tag);
        if (exists)
        {
            image.SetActive(false);
        }
        currentTag = "";
    }

    void Update()
    {
        if(currentTag == "Elektryk" && Input.GetKeyDown(KeyCode.E))
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
        if (currentTag == "Cube_Exit" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Exiting Cube Office");
            PlayerPrefs.SetFloat("Interaction_x", -45.5f);
            PlayerPrefs.SetFloat("Interaction_y", 23.5f);

            SceneManager.LoadScene("Elektryk");
        }
    }
}
