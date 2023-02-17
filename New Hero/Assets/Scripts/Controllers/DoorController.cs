using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    public string sceneName;
    [Header("0 X, 1 Y")]
    public float[] coordinates = new float [2];
    public bool locked = false;
    public UnityEvent onLocked;
}
