using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Text.RegularExpressions;
public class EditorTheme
{
    static readonly List<Theme> themes = new();
    public static List<string> themeNames = new();
    public static Theme currentTheme;
    private static bool dataRead = false;
    private static readonly string themePath = Application.dataPath + "/data/";

    public static void ReadFiles()
    {
        if (dataRead) return;
        DirectoryInfo dir = new(themePath);
        FileInfo[] info = dir.GetFiles("*.xml");
        foreach (FileInfo f in info)
        {
            XmlDocument doc = new();
            doc.Load(f.FullName);

            XmlElement root = doc.DocumentElement;
            string name = root.SelectSingleNode("/theme-data/name").InnerText;
            Color32 background = HexToColor(root.SelectSingleNode("/theme-data/background-color").InnerText);
            Dictionary<Regex, Color32> highlight = new();
            foreach (XmlNode node in root.SelectSingleNode("/theme-data/highlights").ChildNodes)
            {
                Regex regex = new(node.ChildNodes[0].InnerText);
                Color32 color = HexToColor(node.ChildNodes[1].InnerText);

                highlight.Add(regex, color);
            }

            themes.Add(new Theme(name, highlight, background));
            themeNames.Add(name);
        }
        dataRead = true;
        if (PlayerPrefs.HasKey("editorTheme"))
            currentTheme = GetThemeByName(PlayerPrefs.GetString("editorTheme"));
        else
            currentTheme = GetThemeByName("Default");
    }

    public static Theme GetThemeByName(string name)
    {
        return themes.Find(theme => theme.name == name);
    }

    static Color32 HexToColor(string hex)
    {
        hex = hex.Replace("0x", "");
        hex = hex.Replace("#", "");
        byte a = 255;
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
}
