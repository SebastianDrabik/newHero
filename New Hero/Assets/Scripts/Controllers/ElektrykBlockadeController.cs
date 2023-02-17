using UnityEngine;

public class ElektrykBlockadeController : MonoBehaviour
{
    public GameObject markCubeEntrance;
    public DoorController classDoors;

    void Start()
    {
        if(SaveSystem.level >= SaveData.Level.CPP_BASICS)
        {
            gameObject.SetActive(false);
            classDoors.locked = false;
        }
        else 
            markCubeEntrance.SetActive(false);
    }
}
