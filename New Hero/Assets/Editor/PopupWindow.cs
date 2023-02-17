using UnityEditor;
using UnityEngine;

public class PopupWindow : EditorWindow
{
    [MenuItem("Dev/Set compiler path")]
    public static void ShowWindow()
    {
        GetWindow<PopupWindow>();
    }

    private string inputText = "";

    private void OnGUI()
    {
        inputText = EditorGUILayout.TextField("Your compiler path:", inputText);
        if (GUILayout.Button("Submit"))
        {
            Code.compilerPath = inputText;
            Debug.Log($"New compiler path: {inputText}");
        }
    }
}