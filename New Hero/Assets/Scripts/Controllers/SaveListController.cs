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

    private readonly Regex availableCharacters = new("^[0-9A-Za-z_\\-]+$");
    private string saveDataPath;
    private string savesDataPathBase;

    private List<SaveItemData> saves = new();
    
    private enum nameValidState
    {
        CORRECT = 0,
        ALREADY_IN_USE = 1,
        INCORRECT = 2,
    }

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        saveDataPath = $"{Application.persistentDataPath}/saves.xml";
        savesDataPathBase = $"{Application.persistentDataPath}/saves";
        if (!Directory.Exists(savesDataPathBase))
            Directory.CreateDirectory(savesDataPathBase);
        LoadSaves();
    }

    void Start()
    {
        FillList();
    }

    public void CreateSave()
    {
        string name = saveName.text;
        nameValidState valid = IsSaveNameValid(name);
        if(valid != 0)
        {
            string error = "";
            if (valid == nameValidState.ALREADY_IN_USE)
            {
                error = "The name is already in use.";
                Debug.Log("<color=orange>Name has been used before</color>");
            }
            if (valid == nameValidState.INCORRECT)
            {
                error = "Name contains illegal characters.";
                Debug.Log("<color=orange>Name contains illegal characters</color>");
            }
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

    public void RemoveSave(SaveItemData data)
    {
        if(File.Exists(data.Path))
            File.Delete(data.Path);
        saves.Remove(data);
        SaveSaves();
    }

    private nameValidState IsSaveNameValid(string name)
    {
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
}