using UnityEngine;
using TMPro;

public class FightManager : MonoBehaviour
{
    public FightManager instance { get; set; }
    public void Awake()
    {
        if(instance == null)
            instance = this;
    }

    [Space]
    [Header("Top text gameobject")]
    public TextMeshProUGUI top;
    [Header("Bottom text gameobject")]
    public TextMeshProUGUI bottom;
    [Space]
    [Header("Bottom text gameobject")]
    public TMP_InputField codeInput;
    private string correctCode;

    public void OpenCodeEditor(string top, string bottom, string correctCode)
    {
        this.correctCode = correctCode;
        this.top.text = top;
        this.bottom.text = bottom;
        gameObject.SetActive(true);
    }

    public void RunCode()
    {
        Debug.Log(this.codeInput.text.Trim() == this.correctCode);
//        return this.codeInput.text.Trim() == this.correctCode;
    }
    public bool CheckCode()
    {
        return this.codeInput.text.Trim() == this.correctCode;
    }
    public void CloseCodeEditor()
    {
        gameObject.SetActive(false);
    }
}
