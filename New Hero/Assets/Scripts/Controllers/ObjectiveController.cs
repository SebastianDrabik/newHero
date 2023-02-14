using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveController : MonoBehaviour
{
    public TextMeshProUGUI objectiveDescription;
    public GameObject objective;

    public void ShowObjective(string text)
    {
        objectiveDescription.text = text;
        objective.SetActive(true);
    }

    public void HideObjective()
    {
        objective.SetActive(false);
    }
}
