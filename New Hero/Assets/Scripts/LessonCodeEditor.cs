using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class LessonCodeEditor : MonoBehaviour
{
    public TextMeshProUGUI codeText;

    public void Change(string code)
    {
        codeText.text = code;
    }

    private void Update()
    {
        HighlightSyntax(codeText);
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
                    if (vertexColors != null)
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
