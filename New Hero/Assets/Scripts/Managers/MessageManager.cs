using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public MessageController _MessageController;

    public void ShowMessage(string messageKey)
    {
        _MessageController.ShowMessage(TranslationsManager.GetTranslation("messages", messageKey));
    }
}

