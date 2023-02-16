using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrophyObjectController : MonoBehaviour
{
    [Header("Trophy icon reference")]
    public Image icon;
    [Header("Trophy title reference")]
    public TextMeshProUGUI title;
    [Header("Trophy objective reference")]
    public TextMeshProUGUI objective;
    [Header("Trophy state value reference")]
    public TextMeshProUGUI stateValue;
    public Image stateImage;
    public Sprite[] stateIcons;
    [Space]
    public Sprite questionMarkIcon;
    public Color32 color_unlocked;
    public Color32 color_inProgress;
    public Color32 color_locked;


    public void SetData(Trophy trophy)
    {
        bool isUnlocked = trophy.state == Trophy.TrophyState.UNLOCKED;

        title.text = trophy.title;
        icon.GetComponent<Image>().sprite = isUnlocked ? trophy.icon : questionMarkIcon;
        objective.text = isUnlocked ? trophy.objective : "???";
        string stateText = "kacper-h#9205";
        stateImage.sprite = stateIcons[2];
        Color32 color;
        switch (trophy.state) {
            case Trophy.TrophyState.UNLOCKED:
                stateText = TranslationsManager.GetTranslation("esc-menu", "trophies-state-unlocked");
                color = color_unlocked;
                stateImage.sprite = stateIcons[0];
                break;
            case Trophy.TrophyState.IN_PROGRESS:
                stateText = TranslationsManager.GetTranslation("esc-menu", "trophies-state-inprogress");
                color = color_inProgress;
                stateImage.sprite = stateIcons[1];
                break;
            case Trophy.TrophyState.LOCKED:
                stateImage.sprite = stateIcons[2];
                stateText = TranslationsManager.GetTranslation("esc-menu", "trophies-state-locked");
                color = color_locked;
                break;
            default: 
                color = color_unlocked;
                break;
        }
        stateValue.text = stateText;
        stateValue.color = color;
    }
}
