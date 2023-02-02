using UnityEngine;
using TMPro;

public class SaveItemController : MonoBehaviour
{
    public TextMeshProUGUI saveName;
    public TextMeshProUGUI saveCreationDate;
    private SaveItemData data;
    private string savePath;

    public void SetData(SaveItemData data)
    {
        saveName.text = data.Name;
        saveCreationDate.text = data.CreationDate;
        savePath = data.Path;
        this.data = data;
    }

    public void UpdateSaveName(string newName)
    {
        saveName.text = newName;
    }

    public void StartGame()
    {
        SaveGame.StartGame(savePath);
    }

    public void RemoveSave()
    {
        SaveListController controller = GameObject.FindObjectOfType<SaveListController>();
        if (controller != null)
            controller.OpenRemoveSaveModal(data, gameObject);
    }

    public void RenameSave()
    {
        SaveListController controller = GameObject.FindObjectOfType<SaveListController>();
        if (controller != null)
            controller.OpenRenameSaveModal(data, this);
    }
}
