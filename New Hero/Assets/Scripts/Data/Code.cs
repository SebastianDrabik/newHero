using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

[Serializable]
public class Code
{
    private readonly string directoryPath = $"{Application.persistentDataPath}/tmp";
    private readonly string outputFilePath = $"{Application.persistentDataPath}/tmp/pablo.exe";
    private readonly string inputFilePath = $"{Application.persistentDataPath}/tmp/usercode.cpp";
    public static string compilerPath = $"{Application.dataPath}/MinGW/bin/g++.exe";

    
    public string code;
    public Dictionary<string, string> outputs;
    public CodeData.CheckType checkType;

    public Code(string code, Dictionary<string, string> outputs, CodeData.CheckType checkType)
    {
        this.code = code;
        this.outputs = outputs;
        this.checkType = checkType;
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

    private bool Compile(ErrorController controller)
    {
        ClearDirectory();
        CreateFile();
        UnityEngine.Debug.Log(compilerPath + "-o \"" + outputFilePath + "\" " + "\"" + inputFilePath + "\"");
        string error = "";
        var task = Task.Factory.StartNew(()=>
        {
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
            error = process.StandardError.ReadToEnd();

            process.WaitForExit();
            return process.ExitCode;
        });

        task.Wait();

        if (task.Result == 0)
        {
            UnityEngine.Debug.Log("Compilation succeeded");
            return true;
        }
        else
        {
            error = error.Replace(inputFilePath + ": ", "").Replace(inputFilePath + ":", "");
            controller.SetData(error);
            UnityEngine.Debug.LogError("Compilation failed: " + error);
            return false;
        }
    }

    public bool CheckOutputs(Animator animator, ErrorController controller)
    {
        bool compilationResult = Compile(controller);
        if (!compilationResult)
            return false;
        if (!File.Exists(outputFilePath))
        {
            UnityEngine.Debug.LogWarning("Executable file doesn't exist");
            return false;
        }   
        bool result = false;
        animator.SetBool("Spin", true);
        var tasks = new List<Task<bool>>();
        foreach (var item in outputs)
        {
            string input = item.Key,
                output = item.Value;

            tasks.Add(Task.Factory.StartNew(() =>
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = outputFilePath,
                        Arguments = input,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                string processOutput = process.StandardOutput.ReadToEnd();
                UnityEngine.Debug.Log($"<color=blue>Process output: {processOutput}</color>");
                if(checkType == CodeData.CheckType.EXIT_CODE)
                    return process.ExitCode.ToString() == output;
                else
                    return processOutput == output;
            }));
        }

        Task.WaitAll(tasks.ToArray());

        result = tasks.All(x => x.Result);
        //Task.WhenAll(tasks);
        UnityEngine.Debug.Log(result);
        UnityEngine.Debug.Log($"<color=yellow>Code result: {result} </color>");
        animator.SetBool("Spin", false);
        ClearDirectory();

        return result;
    }
}
