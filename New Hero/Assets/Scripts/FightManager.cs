using System.Collections.Generic;
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

    private List<CodeData> codeList = new();
    private string currentKey;

    void Awake()
    {
        LoadAllCodes();
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenCodeEditor(string key)
    {
        CodeData code = GetCode(key);
        gameObject.SetActive(true);
        this.top.text = code.topCode;
        this.bottom.text = code.bottomCode;
        codeInput_text.text = code.initialCode;
        Workspace.color = EditorTheme.currentTheme.background;
        codeInput.Select();
        currentKey = key;
    }

    private void Update()
    {
        HighlightSyntax(top);
        HighlightSyntax(bottom);
        HighlightSyntax(codeInput_text);
    }
    
    private CodeData GetCode(string key)
    {
        return codeList.Find(code => code.key == key);
    }

    public bool CheckCode()
    {
        CodeData cd = GetCode(currentKey);
        //.Replace("\U0000200b", "")
        Code code = new(top.text + "\n" + codeInput.text + "\n" + bottom.text, cd.GetData());
        return code.CheckOutputs();
    }
    public void CloseCodeEditor()
    {
        gameObject.SetActive(false);
        codeInput.text = "";
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

    private void LoadAllCodes()
    {
        CodeData[] datas = Resources.LoadAll<CodeData>("CodeData");
        foreach (var data in datas)
            codeList.Add(data);
    }
}