using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrophyUnlockedNotification : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image icon;
    [Tooltip("Show time in seconds")]
    public float showTime;

    public void Show(Trophy trophy)
    {
        title.text = trophy.title;
        description.text = trophy.objective;
        icon.sprite = trophy.icon;
        Destroy(gameObject, showTime);
    }
}
