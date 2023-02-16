using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ErrorController : MonoBehaviour
{
    public TextMeshProUGUI errorMessage;
    public UnityEvent onOk;

    public void SetData(string errorMessage)
    {
        this.errorMessage.text = errorMessage;
        gameObject.SetActive(true);
    }

    public void HandleOK()
    {
        onOk.Invoke();
    }
}