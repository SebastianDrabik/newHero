using UnityEditor;
using UnityEngine;
using System.Collections;

// Creates an instance of a primitive depending on the option selected by the user.
public class EditorGUILayoutPopup : EditorWindow
{
    public string[] options = new string[] { "Unlocked", "Locked", "InProgress" };
    public int index = 0;
    [MenuItem("Dev/Edit Trophies")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(EditorGUILayoutPopup));
        window.Show();
    }

    void OnGUI()
    {
        index = EditorGUILayout.Popup(index, options);
        GUILayout.TextField("");
        if (GUILayout.Button("Update"))
            InstantiatePrimitive();
    }

    void InstantiatePrimitive()
    {
        switch (index)
        {
            case 0:
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = Vector3.zero;
                break;
            case 1:
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = Vector3.zero;
                break;
            case 2:
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.position = Vector3.zero;
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }
    }
}