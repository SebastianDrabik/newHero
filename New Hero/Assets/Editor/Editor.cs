using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
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

    [MenuItem("Dev/Use default G++ compiler")]
    static void SetGPPPath()
    {
        Code.compilerPath = "g++";
        Debug.Log("Your compiler path has been set to <color=green>g++</color>");
    }
}
