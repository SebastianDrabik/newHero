using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class TranslationsManager
{
    public readonly static string defaultLang = "pl";
    public static string lang = "pl";

    private static readonly List<Lang> langList = new();
    public static List<string> langNames = new();
    public static List<TrophyList> trophyLists = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadLangs()
    {
        TextAsset[] langs = Resources.LoadAll<TextAsset>("Langs");
        foreach (TextAsset lang in langs)
        {
            XmlDocument doc = new();
            doc.LoadXml(lang.text);
            XmlElement root = doc.DocumentElement;
            Dictionary<string, Dictionary<string, string>> translations = new();
            string name=root.SelectSingleNode("lang-info/name").InnerText;
            string key=root.SelectSingleNode("lang-info/key").InnerText;
            foreach (XmlNode group in root.SelectSingleNode("lang-data").ChildNodes)
            {
                string cg = group.Name;
                Dictionary<string, string> temp = new();
                foreach (XmlNode translation in group.ChildNodes)
                {
                    temp.Add(translation.Name, translation.InnerText);
                }
                translations.Add(cg, temp);
            }
            Lang l = new(name, key, translations);
            langList.Add(l);
            langNames.Add(name);
        }
        TrophyList[] trophy = Resources.LoadAll<TrophyList>("Trophies");
        foreach (TrophyList tl in trophy)
            trophyLists.Add(tl);

    }

    public static void UpdateSaveListErrorMessages()
    {
        SaveListController.errorMessages = new()
        {
            { SaveListController.nameValidState.ALREADY_IN_USE, TranslationsManager.GetTranslation("saves-menu", "error-name-already-in-use") },
            { SaveListController.nameValidState.INCORRECT, TranslationsManager.GetTranslation("saves-menu", "error-invalid-characters") },
            { SaveListController.nameValidState.EMPTY, TranslationsManager.GetTranslation("saves-menu", "error-empty") }
        };
    }

    public static string GetKeyByName(string name)
    {
        return langList.Find(l => l.name.Equals(name)).key;
    }

    public static string GetNameByKey(string key)
    {
        return langList.Find(l => l.key.Equals(key)).name;
    }

    public static string GetTranslation(string group, string key)
    {
        return langList.Find(l => l.key == lang).translations[group][key];
    }

    public static TrophyList GetTrophies()
    {
        return trophyLists.Find(tl => tl.langKey == lang);
    }
}
