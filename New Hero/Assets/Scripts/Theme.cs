using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Theme
{
    public string name { get; private set; }
    public Dictionary<Regex, Color32> highlight { get; private set; }
    public Color32 background { get; private set; }


    public Theme(string name, Dictionary<Regex, Color32> highlight, Color32 background)
    {
        this.name = name;
        this.highlight = highlight;
        this.background = background;
    }
}
