using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class FightManager : MonoBehaviour
{
    public FightManager Instance { get; set; }

    private readonly Dictionary<List<string>, Color32> highlightingKeyWordDictionary = new()
    {
        {
            new List<string> { "int", "char", "void", "using" },
            new Color32(0x00, 0x5b, 0xd1, 255)
        },
        {
            new List<string> { "namespace", "main" },
            new Color32(0xd1, 0x73, 0x00, 255)
        },
        {
            new List<string> { "return", "if", "while", "for", "do" },
            new Color32(0xac, 0x77, 0xdd, 255)
        },
        {
            new List<string> { "std", "string" },
            new Color32(0x50, 0xa5, 0x79, 255)
        }
    };

    private readonly Dictionary<Regex, Color32> highlightingRegexDictionary = new()
    {
        { //number
            new Regex("[0-9]+", RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Color32(0xe2, 0xf0, 0x00, 255)
        },
        {
            new Regex("#include\\s*<.*>", RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Color32(0xd1, 0x73, 0x00, 255)
        },
        { //string
            new Regex("\".*\"", RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Color32(0xe7, 0xf7, 0x00, 255)
        },
        { //block comment 
            new Regex(@"/\*.*\*/", RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Color32(0x23, 0x55, 0x30, 255)
        },
        { //comment 
            new Regex(@"//.*$", RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Color32(0x23, 0x55, 0x30, 255)
        },
    };




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

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OpenCodeEditor(string top, string bottom, string correctCode, string defaultCode = "")
    {
        gameObject.SetActive(true);
        this.correctCode = correctCode;
        this.top.text = top;
        this.bottom.text = bottom;
        codeInput_text.text = defaultCode;
        codeInput.Select();
    }

    public void RunCode()
    {
        Debug.Log(RemoveAllSpaces(this.codeInput.text) == RemoveAllSpaces(this.correctCode));
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
        if (text == null) return;
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
        foreach (var item in highlightingRegexDictionary)
        {
            Color32 color = item.Value;
            foreach (Match match in item.Key.Matches(text.text))
            {
                //Debug.Log("<color=yellow>" + match.Value + "</color> <color=red>" + match.Index + "</color> <color=cyan>" + text.textInfo.characterInfo.Length + "</color>");
                for (int i = match.Index; i < match.Index + match.Length; i++)
                {
                    if (i > text.textInfo.characterInfo.Length) break;
                    TMP_CharacterInfo characterInfo = text.textInfo.characterInfo[i];
                    int meshIndex = text.textInfo.characterInfo[characterInfo.index].materialReferenceIndex;
                    int vertexIndex = text.textInfo.characterInfo[characterInfo.index].vertexIndex;

                    Color32[] vertexColors = text.textInfo.meshInfo[meshIndex].colors32;
                    vertexColors[vertexIndex + 0] = color;
                    vertexColors[vertexIndex + 1] = color;
                    vertexColors[vertexIndex + 2] = color;
                    vertexColors[vertexIndex + 3] = color;
                }
            }
        }
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    private Color32 FindColor(string word)
    {
        Color32 color = new(0xff, 0xff, 0xff, 0xff);
        foreach (var item in highlightingKeyWordDictionary)
        {
            if (item.Key.Contains(word))
                color = item.Value;
        }
        return color;
    }
}