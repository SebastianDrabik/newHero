using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[Serializable]
public class Code
{
    private readonly string directoryPath = $"{Application.persistentDataPath}/tmp";
    private readonly string outputFilePath = $"{Application.persistentDataPath}/tmp/pablo.exe";
    private readonly string inputFilePath = $"{Application.persistentDataPath}/tmp/usercode.cpp";
    public static string compilerPath = $"{Application.dataPath}/MinGW/bin/g++.exe";

    
    public string code;
    public Dictionary<string, string> outputs;

    public Code(string code, Dictionary<string, string> outputs)
    {
        this.code = code;
        this.outputs = outputs;
    }

    public Code(string code, string input, string output)
    {
        this.code = code;
        outputs.Add(input, output);
    }

    private void CreateFile()
    {
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        if (File.Exists(inputFilePath))
            File.Delete(inputFilePath);
        using StreamWriter sw = File.CreateText(inputFilePath);
        foreach (string codeLine in code.Split("\n"))
        {
            sw.WriteLine(codeLine);
        }
        sw.Close();
    }

    private void ClearDirectory()
    {
        if (Directory.Exists(directoryPath))
        {
            if (File.Exists(inputFilePath))
                File.Delete(inputFilePath);
            if(File.Exists(outputFilePath))
                File.Delete(outputFilePath);
        }
    }

    private void Compile()
    {
        ClearDirectory();
        CreateFile();
        UnityEngine.Debug.Log(compilerPath + "-o \"" + outputFilePath + "\" " + "\"" + inputFilePath + "\"");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "\"" + compilerPath + "\"",
                Arguments = "-o \"" + outputFilePath + "\"" + " \"" + inputFilePath + "\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            UnityEngine.Debug.Log("Compilation succeeded");
        }
        else
        {
            UnityEngine.Debug.LogError("Compilation failed: " + error);
        }
    }

    public bool CheckOutputs()
    {
        Compile();
        if (!File.Exists(outputFilePath))
        {
            UnityEngine.Debug.LogWarning("Executable file doesn't exist");
            return false;
        }   
        bool result = false;

        foreach (var item in outputs)
        {
            string input = item.Key,
                output = item.Value;

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = outputFilePath,
                    Arguments = input,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                }
            };

            process.Start();

            string processOutput = process.StandardOutput.ReadToEnd();
            
            if (process.ExitCode.ToString() == output)
                result = true;
            else
            {
                result = false;
                break;
            }
        }
        UnityEngine.Debug.Log($"<color=yellow>Code result: {result} </color>");

        ClearDirectory();

        return result;
    }
}
