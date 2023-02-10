using System;
using System.Collections.Generic;

[Serializable]
public class Lang
{
    public string name { get; private set; }
    public string key { get; private set; }
    //                group             key       value
    public Dictionary<string, Dictionary<string, string>> translations { private set; get; } = new();

    public Lang(string name, string key, Dictionary<string, Dictionary<string, string>> translations)
    {
        this.name = name;
        this.key = key;
        this.translations = translations;
    }
}