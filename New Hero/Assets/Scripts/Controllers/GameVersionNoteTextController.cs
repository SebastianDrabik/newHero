using UnityEngine;
using TMPro;

public class GameVersionNoteTextController : MonoBehaviour
{
    //TODO
    private Color32 initialColor;

    private TextMeshProUGUI text;
    public Color32 blinkColor;

    private int currentIndex = 0;
    private int maxIndex;
    private float timer = 0.2f;

    void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        maxIndex = text.text.Length - 1;
    }

    void Start()
    {
        initialColor = text.color;
    }

    void Update()
    {
        if (timer <= 0)
        {
            if (currentIndex < maxIndex)
                currentIndex++;
            else
                currentIndex = 0;
            timer = 0.2f;
        }
        TMP_CharacterInfo characterInfo = text.textInfo.characterInfo[currentIndex];
        int meshIndex = text.textInfo.characterInfo[characterInfo.index].materialReferenceIndex;
        int vertexIndex = text.textInfo.characterInfo[characterInfo.index].vertexIndex;

        Color32[] vertexColors = text.textInfo.meshInfo[meshIndex].colors32;
        if (vertexColors != null)
        {
            vertexColors[vertexIndex + 0] = blinkColor;
            vertexColors[vertexIndex + 1] = blinkColor;
            vertexColors[vertexIndex + 2] = blinkColor;
            vertexColors[vertexIndex + 3] = blinkColor;
        }

        int prevIndex;
        if (currentIndex < 1)
            prevIndex = maxIndex;
        else 
            prevIndex = currentIndex - 1;
        if(prevIndex > 0 && prevIndex <= maxIndex)
        {

            TMP_CharacterInfo characterInfoPrevious = text.textInfo.characterInfo[prevIndex];
            int meshIndexPrevious = text.textInfo.characterInfo[characterInfoPrevious.index].materialReferenceIndex;
            int vertexIndexPrevious = text.textInfo.characterInfo[characterInfoPrevious.index].vertexIndex;

            Color32[] vertexColorsPrevious = text.textInfo.meshInfo[meshIndexPrevious].colors32;
            if (vertexColorsPrevious != null)
            {
                vertexColorsPrevious[vertexIndexPrevious + 0] = initialColor;
                vertexColorsPrevious[vertexIndexPrevious + 1] = initialColor;
                vertexColorsPrevious[vertexIndexPrevious + 2] = initialColor;
                vertexColorsPrevious[vertexIndexPrevious + 3] = initialColor;
            }
        }
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        timer -= Time.deltaTime;
    }
}

