using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;

public class SaveListController : MonoBehaviour
{
    public static SaveListController Instance;

    public TMP_InputField saveName;
    public GameObject saveListItemPrefab;
    public Transform saveListItemContainer;
    public SaveDeleteController modalAccept;

    public GameObject modalCreate;
    public TextMeshProUGUI errorMessage;

    public SaveRenameController modalRename;

    private readonly Regex availableCharacters = new("^[0-9A-Za-z_\\-]+$");
    private string saveDataPath;
    private string savesDataPathBase;

    private List<SaveItemData> saves = new();
    
    public enum nameValidState
    {
        CORRECT = 0,
        ALREADY_IN_USE = 1,
        INCORRECT = 2,
        EMPTY = 3,
    }

    public readonly Dictionary<nameValidState, string> errorMessages = new()
    {
        { nameValidState.ALREADY_IN_USE, "Name is already in use." },
        { nameValidState.INCORRECT, "Name contains illegal characters." },
        { nameValidState.EMPTY, "Name cannot be empty." }
    };

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        saveDataPath = $"{Application.persistentDataPath}/saves.xml";
        savesDataPathBase = $"{Application.persistentDataPath}/saves";
        if (!Directory.Exists(savesDataPathBase))
            Directory.CreateDirectory(savesDataPathBase);
    }

    void Start()
    {
        LoadSaves();
        FillList();
    }

    public void CreateSave()
    {
        string name = saveName.text;
        nameValidState valid = IsSaveNameValid(name);
        if(valid != 0)
        {
            string error = errorMessages[valid];
            errorMessage.text = error;
            errorMessage.gameObject.SetActive(true);
            return;
        }
        string now = DateTime.Now.ToString("MM/dd/yyyy");

        SaveItemData item = new();
        item.Name = name;
        item.CreationDate = now;
        item.Path = $"{savesDataPathBase}/{name}.tcg";
        saves.Add(item);
        AddListItem(item);
        SaveSaves();
        Debug.Log("<color=green>Successfully added new save</color>");
        saveName.text = String.Empty;
        modalCreate.SetActive(false);
    }

    private void SaveSaves()
    {
        XmlSerializer x = new(saves.GetType());

        FileStream fs = new(saveDataPath, FileMode.Create);
        x.Serialize(fs, saves);
        fs.Close();
    }

    private void LoadSaves()
    {
        if (!File.Exists(saveDataPath))
        {
            Debug.LogWarning("Save list file not found in " + saveDataPath);
            return;
        }
        FileStream fs = new(saveDataPath, FileMode.Open);
        XmlSerializer x = new(saves.GetType());
        saves = x.Deserialize(fs) as List<SaveItemData>;
    }

    private void FillList()
    {
        foreach (SaveItemData item in saves)
        {
            AddListItem(item);
        }
    }

    private void AddListItem(SaveItemData item)
    {
        GameObject data = Instantiate(saveListItemPrefab, saveListItemContainer);
        data.GetComponent<SaveItemController>().SetData(item);
    }

    public void RenameSave(SaveItemData data, string newName)
    {
        SaveItemData sid = saves.Find(save => save.Name == data.Name);
        if (sid == null)
            return;
        sid.Name = newName;
        if (File.Exists(sid.Path))
        {
            FileInfo fileInfo = new(sid.Path);
            fileInfo.MoveTo(fileInfo.Directory.FullName + "\\" + newName + ".tcg");
        }
        sid.Path = $"{savesDataPathBase}/{newName}.tcg";
        SaveSaves();
    }

    public void RemoveSave(SaveItemData data)
    {
        if(File.Exists(data.Path))
            File.Delete(data.Path);
        saves.Remove(data);
        SaveSaves();
    }

    public nameValidState IsSaveNameValid(string name)
    {
        if (name.Length == 0)
            return nameValidState.EMPTY;
        if (!availableCharacters.IsMatch(name))
            return nameValidState.INCORRECT;
        if (saves.Find(save => save.Name == name) != null)
            return nameValidState.ALREADY_IN_USE;
        return nameValidState.CORRECT;
    }

    public void OpenRemoveSaveModal(SaveItemData data, GameObject UIElement)
    {
        modalAccept.OpenModal(data, UIElement);
    }

    public void OpenRenameSaveModal(SaveItemData data, SaveItemController controller)
    {
        modalRename.OpenModal(data, controller);
    }

    public void SearchSave(string name)
    {
        if(name != String.Empty)
        {
            foreach (Transform item in saveListItemContainer)
            {
                SaveItemData data = item.gameObject.GetComponent<SaveItemController>().data;
                Regex searchRegex = new($".*{name}.*", RegexOptions.IgnoreCase);
                if(searchRegex.IsMatch(data.Name))
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            }
            return;
        }
        foreach (Transform item in saveListItemContainer)
        {
            item.gameObject.SetActive(true);
        }
    }
}