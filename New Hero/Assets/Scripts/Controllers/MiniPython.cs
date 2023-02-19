using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPython : MonoBehaviour
{
    public FightManager editor;
    public GameObject python;


    public void AttackPlayer(int codeIndex)
    {
        IEnumerator attackC = Attack(codeIndex);
        StartCoroutine(attackC);
    }

    IEnumerator Attack(int i)
    {
        yield return new WaitForSeconds(0.25f);
        Debug.Log($"python{i}");
        editor.OpenCodeEditor($"python{i}");
    }
}
