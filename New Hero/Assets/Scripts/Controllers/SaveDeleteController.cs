using TMPro;
using UnityEngine;

public class SaveDeleteController : MonoBehaviour
{
    private SaveItemData data;
    private GameObject UIElement;

    public TextMeshProUGUI modalContent;

    public void OpenModal(SaveItemData data, GameObject UIElement)
    {
        gameObject.SetActive(true);
        this.data = data;
        modalContent.text = $"Are you sure you want to delete save: {data.Name}?";
        this.UIElement = UIElement;
    }

    public void RemoveSave()
    {
        SaveListController.Instance.RemoveSave(data);
        Destroy(this.UIElement);
        gameObject.SetActive(false);
        Debug.Log("<color=green>Save was successfully removed</color>");
    }
}
