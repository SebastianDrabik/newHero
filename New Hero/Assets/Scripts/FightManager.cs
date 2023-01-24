using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightManager : MonoBehaviour
{
    public FightManager Instance { get; set; }
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
    [Header("Workspace gameobject")]
    public Image Workspace;
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
        Workspace.color = EditorTheme.currentTheme.background;
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
        if (text == null || text.text.Length == 0) return;
        foreach (var item in EditorTheme.currentTheme.highlight)
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
                    if(vertexColors != null)
                    {
                        vertexColors[vertexIndex + 0] = color;
                        vertexColors[vertexIndex + 1] = color;
                        vertexColors[vertexIndex + 2] = color;
                        vertexColors[vertexIndex + 3] = color;
                    }
                }
            }
        }
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}