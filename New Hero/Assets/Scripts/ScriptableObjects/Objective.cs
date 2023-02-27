using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Scriptable Objects/Objective", fileName = "NewObjective")]
public class Objective : ScriptableObject
{
    [Header("Objective Unique Key")]
    public string objectiveKey;
    public string translationKey;
    public bool pointPosition;
    [Header("0 - X, 1 - Y")]
    public float[] position;
}