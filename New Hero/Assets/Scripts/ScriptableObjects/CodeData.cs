using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CodeData", menuName = "Scriptable Objects/Code")]
public class CodeData : ScriptableObject
{
    [Serializable]
    public struct inputData
    {
        public string input;
        public string output;
    }

    [Header("Unique code object key")]
    public string key;
    [TextArea(3,10)]
    public string topCode;
    [TextArea(3, 10)]
    public string bottomCode;
    [TextArea(3, 10)]
    public string initialCode = "";
    
    [Header("All correct outputs for inputs")]
    public inputData[] inputOutput;

    //TODO modes - return/output

    public Dictionary<string, string> GetData()
    {
        Dictionary<string, string> temp = new();
        for (int i = 0; i < inputOutput.Length; i++)
        {
            temp.Add(inputOutput[i].input, inputOutput[i].output);
        }
        return temp;
    }
}

