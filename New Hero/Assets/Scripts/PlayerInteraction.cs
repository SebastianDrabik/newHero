using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject image;

    public string[] tags = {"Elektryk", "Elektryk_Exit" };

    private string currentTag="";

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
            SceneManager.LoadScene("Elektryk");
        }
        if (currentTag == "Elektryk_Exit" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Exiting Elektryk");
            SceneManager.LoadScene("Demo");
        }
    }
}
