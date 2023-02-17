using UnityEngine;
using TMPro;
using System.Collections;

public class MessageController : MonoBehaviour 
{
    public TextMeshProUGUI message;
    public float showTime;

    private bool shown = false;

    public void ShowMessage(string _message)
    {
        if (!shown)
        {
            gameObject.SetActive(true);
            message.text = _message;
            Invoke(nameof(HideMessage), showTime);
            shown = true;
        }
    }

    private void HideMessage()
    {
        if (shown)
        {
            gameObject.SetActive(false);
            shown = false;
        }
    }
}

