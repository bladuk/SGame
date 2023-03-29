using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class EditorFileDialog : MonoBehaviour
{
    [SerializeField] private TMP_InputField _fileNameInputField;

    public void CreateFile()
    {
        if (_fileNameInputField == null)
        {
            Debug.LogError("File name input field is null!");
            return;
        }
        
        if (_fileNameInputField.text.Length <= 1)
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Warning, "Внимание", "Вы не ввели название файла!");
            return;
        }

        string outputFilePath = Path.Combine(QuestionsBrowser.Singleton.QuestionsFolder, _fileNameInputField.text + ".sgamesq");
        
        if (File.Exists(outputFilePath))
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Файл с таким названием уже существует.");
            _fileNameInputField.text = "";
            return;
        }
        
        File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(GameParser.Singleton.SequenceTemplate, Formatting.Indented));
        EditorScript.Singleton.LoadFromFile(outputFilePath);
        gameObject.SetActive(false);
        Toast.Singleton.ShowToast(Toast.ToastMessageType.Information, "Успех", "Файл успешно создан.");
    }
}
