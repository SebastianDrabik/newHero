using UnityEngine;

public class LessonManager : MonoBehaviour
{
    public GameObject classExit;
    private GameManager manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (SaveSystem.level >= SaveData.Level.CPP_BASICS)
        {
            gameObject.SetActive(false);
            return;
        }
        manager.ShowObjective("lesson-seat");
        classExit.SetActive(false);
    }
}
