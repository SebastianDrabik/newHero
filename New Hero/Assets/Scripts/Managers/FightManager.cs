using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class FightManager : MonoBehaviour
{
    public FightManager Instance { get; set; }

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
    public Animator runButtonAnimator;
    public Button runButton;

    public GameObject hintButton;

    public ErrorController errorController;
    public UnityEvent onHintClicked;

    private PlayerMovement movement;
    private PlayerInteraction interaction;

    private PauseMenu pauseMenu;

    [Space]
    [Header("Event called after successfull compilation")]
    public UnityEvent<bool> onCodeExecuted;

    private static readonly List<CodeData> codeList = new();
    private static string currentKey;

    private CodeData currentData;

    void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PauseMenu>();
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
        movement = FindObjectOfType<PlayerMovement>();
        interaction = FindObjectOfType<PlayerInteraction>();
    }

    public void OpenCodeEditor(string key)
    {
        currentData = GetCode(key);
        if(currentData == null)
        {
            Debug.LogError($"Cannot find CodeData with key: {key}");
            return;
        }
        gameObject.SetActive(true);
        top.text = currentData.topCode;
        bottom.text = currentData.bottomCode;
        codeInput_text.text = currentData.initialCode;
        Workspace.color = EditorTheme.currentTheme.background;
        codeInput.Select();
        currentKey = key;
        
        pauseMenu.SetDisabled(true);
        interaction.SetInteractionDisabled(true);
        movement.SetMovementDisabled(true);
    }

    private void LateUpdate()
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
        Code code = new(top.text + "\n" + codeInput.text + "\n" + bottom.text, cd.GetData(), cd.checkType);
        
        return code.CheckOutputs(runButtonAnimator, errorController);
    }

    public void CloseCodeEditor()
    {
        pauseMenu.SetDisabled(false);
        codeInput.text = "";
        movement.SetMovementDisabled(false);
        interaction.SetInteractionDisabled(false);
        gameObject.SetActive(false);
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

    public void RunCode()
    {
        Debug.Log("RUN");
        bool codeResult = CheckCode();
        onCodeExecuted.Invoke(codeResult);
        //compile 
        //trigger event
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadAllCodes()
    {
        if(codeList.Count > 0)
            codeList.Clear();
        CodeData[] datas = Resources.LoadAll<CodeData>("CodeData");
        foreach (var data in datas)
            codeList.Add(data);
    }

    public void ShowHint()
    {
        onHintClicked.Invoke();
        DisableHint();
    }

    public void EnableHint()
    {
        hintButton.SetActive(true);
    }

    public void DisableHint()
    {
        hintButton.SetActive(false);
    }
}