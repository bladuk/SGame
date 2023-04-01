using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DeveloperConsole : MonoBehaviour
{
    [SerializeField] private GameObject _console;
    [SerializeField] private TMP_InputField _commandInput;
    [SerializeField] private TMP_Text _output;

    public void OpenLogFile()
    {
#if UNITY_EDITOR
        Application.OpenURL($"file://{Environment.GetEnvironmentVariable("USERPROFILE")}\\AppData\\Local\\Unity\\Editor\\Editor.log");
#else        
        Application.OpenURL($"file://{Environment.GetEnvironmentVariable("USERPROFILE")}\\AppData\\LocalLow\\{Application.companyName.Replace(" ", "")}\\{Application.productName}\\Player.log");   
#endif
    }

    public void ExecuteCommand()
    {
        if (_commandInput == null)
        {
            Debug.LogError("Command input is null!");
            return;
        }

        string commandName = _commandInput.text.Contains(' ') ? _commandInput.text.Split(' ')[0] : _commandInput.text;
        string[] args = _commandInput.text.Replace(commandName, "").TrimStart().Split(' ');
        _commandInput.text = "";
        StartCoroutine(SelectInputField());

        Debug.Log($"Trying to execute command {commandName}. {args.Length} arguments provided.");
        
        AppendOutput($"CLIENT >>> {commandName}");

        switch (commandName)
        {
            case "buildinfo":
                AppendOutput($"Version: {Application.version}\nIs beta: {BuildInfo.Singleton.IsBetaBuild}\nIs development: {BuildInfo.Singleton.IsDevelopmentBuild}\nUnity version: {Application.unityVersion}");
                break;
            case "exit":
                AppendOutput("Exiting the game...");
                Application.Quit();
                break;
            case "exec":
                Type.GetType(args[0]).GetMethod(args[1]).Invoke(null, Array.Empty<object>());
                break;
            case "teams":
                foreach (var team in FindObjectsOfType<TeamPrefab>())
                {
                    AppendOutput($"{team.Id.text}. {team.Name.text}");
                }
                break;
            default:
                AppendOutput($"Error! Command {commandName} not found.");
                Debug.Log($"Command {commandName} not found.");
                break;
        }
    }

    private void AppendOutput(string text)
    {
        if (_output == null)
        {
            Debug.LogError("Commands output is null!");
            return;
        }

        _output.text += $"[{DateTime.Now:HH:mm:ss UTCz}] {text}\n";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            _console.SetActive(!_console.activeSelf);
            StartCoroutine(SelectInputField());
        }
        else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && _console.activeSelf && _commandInput.text.Length > 0)
            ExecuteCommand();
    }

    IEnumerator SelectInputField()
    {
        yield return new WaitForEndOfFrame();
        _commandInput.ActivateInputField();
    }
}
