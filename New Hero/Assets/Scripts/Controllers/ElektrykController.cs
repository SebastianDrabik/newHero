using UnityEngine;
public class ElektrykController : MonoBehaviour 
{
    public DoorController exit;
    public SaveData.Level minLeaveLevel;

    void Start()
    {
        if (SaveSystem.level < minLeaveLevel)
            exit.locked = true;
    }
}