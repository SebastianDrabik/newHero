using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class TranslationsManager
{
    public readonly static string defaultLang = "pl";
    public static string lang = "pl";

    private static List<Lang> langList = new();
    public static List<string> langNames = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadLangs()
    {
        //if (!PlayerPrefs.HasKey("Language"))
        //{
        //    lang = defaultLang;
        //    PlayerPrefs.SetString("Language", defaultLang);
        //}
        //else
        //    lang = PlayerPrefs.GetString("Language");


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
    }

    public static string GetKeyByName(string name)
    {
        return langList.Find(l => l.name.Equals(name)).key;
    }

    public static string GetTranslation(string group, string key)
    {
        return langList.Find(l => l.key == lang).translations[group][key];
    }
}
