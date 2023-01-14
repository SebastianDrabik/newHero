using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class FightManager : MonoBehaviour
{
    public FightManager instance { get; set; }

    private readonly Dictionary<List<string>, Color32> highlightingDictionary = new()
    {
        {
            new List<string> { "int", "char", "void", "using" },
            new Color32(0x00, 0x5b, 0xd1, 255)
        },
        {
            new List<string> { "include", "namespace", "main" },
            new Color32(0xd1, 0x73, 0x00, 255)
        },
        {
            new List<string> { "return", "if", "while", "for", "do" },
            new Color32(0xac, 0x77, 0xdd, 255)
        },
        {
            new List<string> { "std", "string" },
            new Color32(0x50, 0xa5, 0x79, 255)
        },
        { //TODO - multicharacter numbers
            new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" },
            new Color32(0xe7, 0xf7, 0x00, 255)
        }
    };
    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [Space]
    [Header("Top text gameobject")]
    public TextMeshProUGUI top;
    [Header("Bottom text gameobject")]
    public TextMeshProUGUI bottom;
    [Space]
    [Header("User's code input gameobject")]
    public TMP_InputField codeInput;
    [Header("User's code input gameobject - content")]
    public TextMeshProUGUI codeInput_text;
    private string correctCode;

    public void OpenCodeEditor(string top, string bottom, string correctCode, string defaultCode = "")
    {
        this.correctCode = correctCode;
        this.top.text = top;
        this.bottom.text = bottom;
        codeInput_text.text = defaultCode;
        gameObject.SetActive(true);
        HighlightSyntax(this.top);
        HighlightSyntax(this.bottom);
        HighlightSyntax(codeInput_text);
        codeInput.Select();
    }

    public void RunCode()
    {
        Debug.Log(RemoveAllSpaces(this.codeInput.text) == RemoveAllSpaces(this.correctCode));
        //        return this.codeInput.text.Trim() == this.correctCode;
    }
    private void Update()
    {
        HighlightSyntax(top);
        HighlightSyntax(bottom);
        HighlightSyntax(codeInput_text);
    }
    public bool CheckCode()
    {
        return RemoveAllSpaces(this.codeInput.text.Trim()) == RemoveAllSpaces(this.correctCode.Trim());
    }
    public void CloseCodeEditor()
    {
        gameObject.SetActive(false);
        codeInput.text = "";
    }
    private string RemoveAllSpaces(string s)
    {
        string result = "";
        for (int i = 0; i < s.Length; i++)
            if (s[i] != ' ' && s[i] != '\n' && s[i] != '\t' && s[i] != '\r') result += s[i];
        return result;
    }
    private void HighlightSyntax(TextMeshProUGUI text)
    {
        for (int j = 0; j < text.textInfo.wordCount; j++)
        {
            TMP_WordInfo info = text.textInfo.wordInfo[j];
            Color32 color = FindColor(info.GetWord());
            for (int i = 0; i < info.characterCount; ++i)
            {
                int charIndex = info.firstCharacterIndex + i;
                int meshIndex = text.textInfo.characterInfo[charIndex].materialReferenceIndex;
                int vertexIndex = text.textInfo.characterInfo[charIndex].vertexIndex;

                Color32[] vertexColors = text.textInfo.meshInfo[meshIndex].colors32;
                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;
            }
        }

        text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
    //todo
    //void FixCodeIndent(string value)
    //{
    //    if(value == null || value.Length == 0)
    //        return;
    //    List<string> lines = new();
    //    string current = "";
    //    for(int i = 0; i < value.Length; i++)
    //    {
    //        if (value[i] != '\n')
    //        {
    //            current += value[i];
    //            lines.Add(current);
    //        }
    //        else
    //            continue;
    //    }
    //    if (lines.Count <= 1)
    //        return;
    //    string prevLine = lines[^2];
    //    string indent = "";
    //    for(int i = 0; i < prevLine.Length; i++)
    //    {
    //        if (prevLine[i] == '\t')
    //            indent += "\t";
    //        //else
    //          //  break;
    //    }
    //    Debug.Log(indent);
    //    //Debug.Log(codeInput.caretPosition);
    //    //codeInput.caretPosition += indent*4;
    //    string newText = value.Insert(codeInput.caretPosition-1, indent);
    //    codeInput.text = newText;
    //}

    private Color32 FindColor(string word)
    {
        Color32 color = new(0xff, 0xff, 0xff, 0xff);
        foreach (var item in highlightingDictionary)
        {
            if (item.Key.Contains(word))
                color = item.Value;
        }
        return color;
    }
}
