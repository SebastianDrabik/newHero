using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Version", fileName = "New Version")]
public class CurrentGameVersion : ScriptableObject
{
    public string version;
}
