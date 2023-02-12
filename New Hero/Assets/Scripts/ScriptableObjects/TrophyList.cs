using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Trophy List", fileName = "NewTrophyList")]
public class TrophyList : ScriptableObject
{
    public string langKey;
    public List<Trophy> trophies;
}
