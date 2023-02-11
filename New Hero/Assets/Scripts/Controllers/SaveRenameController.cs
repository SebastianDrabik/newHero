using TMPro;
using UnityEngine;

public class SaveRenameController : MonoBehaviour
{
    private SaveItemData data;
    private SaveItemController saveItemController;
    public TMP_InputField newName;
    public TextMeshProUGUI errorMessage;

    public TextMeshProUGUI modalContent;

    public void OpenModal(SaveItemData data, SaveItemController saveItemController)
    {
        gameObject.SetActive(true);
        this.data = data;
        modalContent.text = data.Name;
        this.saveItemController = saveItemController;
    }

    public void RenameSave()
    {
        SaveListController saveListController = SaveListController.Instance;
        SaveListController.nameValidState valid = saveListController.IsSaveNameValid(newName.text);
        if (valid != 0)
        {
            string error = saveListController.errorMessages[valid];
            errorMessage.text = error;
            errorMessage.gameObject.SetActive(true);
            return;
        }
        saveListController.RenameSave(data, newName.text);
        saveItemController.UpdateSaveName(newName.text);
        gameObject.SetActive(false);
        Debug.Log("<color=green>Save has been successfully renamed</color>");
    }
}