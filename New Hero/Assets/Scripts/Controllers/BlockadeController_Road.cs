using UnityEngine;

public class BlockadeController_Road : MonoBehaviour
{
    void Start()
    {
        if(SaveSystem.level >= SaveData.Level.MARK_CUBE)
            gameObject.SetActive(false);
    }
}
