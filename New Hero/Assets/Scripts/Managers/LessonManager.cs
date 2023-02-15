using UnityEngine;
using System.Collections.Generic;

public class LessonManager : MonoBehaviour
{
    public GameObject classExit;
    private GameManager manager;
    //[System.Serializable]
    //struct

    //private List<struct> codes;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (SaveSystem.level >= SaveData.Level.CPP_BASICS)
        {
            //gameObject.SetActive(false);
            return;
        }
        manager.ShowObjective("lesson-seat");
        classExit.SetActive(false);
    }

    public void StartLesson()
    {
          
    }
}
