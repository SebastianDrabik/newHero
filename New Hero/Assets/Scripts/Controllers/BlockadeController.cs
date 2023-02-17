using UnityEngine;

public class BlockadeController : MonoBehaviour
{
    public GameObject markCubeEntrance;
    public DoorController classDoors;

    void Start()
    {
        if(SaveSystem.level >= SaveData.Level.CPP_BASICS)
            gameObject.SetActive(false);
        else
            markCubeEntrance.SetActive(false);

        if (SaveSystem.level >= SaveData.Level.MARK_CUBE)
            classDoors.locked = false;
    }
}
