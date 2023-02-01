using UnityEditor;
using System.IO;

public class Editor : UnityEditor.Editor
{
    //[MenuItem("Dev/Clear Save")]
    //static void ClearSave()
    //{
    //    File.Delete(SaveSystem.path);
    //}

    //[MenuItem("Dev/Load Save")]
    //static void LoadSave()
    //{
    //    SaveGame.StartGame();
    //}

    [MenuItem("Dev/Skip Marco Cube Fight")]
    static void SkipFight()
    {
        MarkCube.Instance.JumpAttack();
    }

    //[MenuItem("Dev/Edit Trophies")]
    //static void Edit()
    //{
    //    int selected = 0;
    //    string[] options = new string[]
    //    {
    // "Option1", "Option2", "Option3",
    //    };
    //    selected = EditorGUILayout.Popup("Label", selected, options);
    //}
    //
    //public List<string> NameList;
    //
    //[MenuItem("Dev/NameList")]
    //public static string MyName;
}
