using UnityEngine;
using TMPro;

public class OutputController : MonoBehaviour
{
    public TextMeshProUGUI outputContent;
    public TextMeshProUGUI outputHeader;

    public Color32 successColor;
    public Color32 failureColor;

    private string translationGroup = "editor";
    private string[] translationKey =
    {
        "title-success",
        "title-failure",
        "output-content",
    };

    public void ShowOutput(CodeResult codeResult)
    {
        Debug.Log("Test");
        gameObject.SetActive(true);
        outputHeader.text = TranslationsManager.GetTranslation(translationGroup, translationKey[codeResult.Correct ? 0 : 1]);
        outputHeader.color = codeResult.Correct ? successColor : failureColor;

        string content = TranslationsManager.GetTranslation(translationGroup, translationKey[2]);
        outputContent.text = content.Replace("%nl%", "\n").Replace("%OUTPUT%", codeResult.Result).Replace("%EXPECTED_OUTPUT%", codeResult.ExpectedResult);

    }
}