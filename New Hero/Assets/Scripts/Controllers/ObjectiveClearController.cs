using System;
using UnityEngine;

public class ObjectiveClearController : MonoBehaviour
{
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Start()
    {
        gameManager.HideObjective();
    }
}