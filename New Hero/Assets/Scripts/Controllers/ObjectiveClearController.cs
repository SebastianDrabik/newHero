using System;
using UnityEngine;

public class ObjectiveClearController : MonoBehaviour
{
    private GameManager gameManager;
    [Header("Keys of objectives that might be removed")]
    public string[] clearList;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Start()
    {
        string obj = gameManager.currentObjective;
        if (obj == null)
            return;
        foreach (string item in clearList)
        {
            if(item == gameManager.currentObjective)
            {
                gameManager.HideObjective();
                break;
            }
        }
    }
}