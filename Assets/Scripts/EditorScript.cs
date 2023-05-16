using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class EditorScript : MonoBehaviour
{
    [SerializeField] private GameObject _editorBase;
    [SerializeField] private TMP_Text _currentFileText;
    [SerializeField] private TMP_InputField _topicInputField;
    [SerializeField] private TMP_InputField _descriptionInputField;
    [SerializeField] private TMP_InputField _backgroundInputField;
    [SerializeField] private GameObject _categoriesViewport;
    [SerializeField] private GameObject _categoryPrefab;

    private string _currentFilePath;
    private Sequence _outputSequence;
    
    internal bool ActionsRequired = false;

    public static EditorScript Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        _topicInputField.onValueChanged.AddListener(delegate { ActionsRequired = true; });
        _descriptionInputField.onValueChanged.AddListener(delegate { ActionsRequired = true; });
        _backgroundInputField.onValueChanged.AddListener(delegate { ActionsRequired = true; });
    }

    internal void OpenEditor() => _editorBase.SetActive(true);

    public void WriteFile()
    {
        if (!File.Exists(_currentFilePath))
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Редактор не подключен ни к одному файлу. Создайте новый или откройте существующий.", 5f);
            return;
        }
        
        _outputSequence = new Sequence("", "", "", new Dictionary<string, List<Question>>());
        
        if (_topicInputField.text.Length <= 1)
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Вы не ввели тему игры. Невозможно сохранить файл.", 5f);
            return;
        }
        
        _outputSequence.Topic = _topicInputField.text;
        
        if (_descriptionInputField.text.Length <= 1)
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Вы не ввели описание игры. Невозможно сохранить файл.", 5f);
            return;
        }
        
        _outputSequence.Description = _descriptionInputField.text;

        if (_backgroundInputField.text.Length > 1)
        {
            _outputSequence.Background = _backgroundInputField.text;
        }

        _outputSequence.Questions = new Dictionary<string, List<Question>>();
        
        for (int i = 0; i < _categoriesViewport.transform.childCount; i++)
        {
            var category = _categoriesViewport.transform.GetChild(i).GetComponent<CategoryPrefab>();
        
            if (category.TitleText.text.Length <= 1)
            {
                Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", $"Вы не ввели тему категории {i + 1}. Невозможно сохранить файл", 5f);
                return;
            }
        
            if (!_outputSequence.Questions.TryAdd(category.TitleText.text, new List<Question>()))
            {
                Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", $"Не удалось добавить категорию {i + 1}. Возможно, категория с таким названием уже существует.", 5f);
                return;
            }
        
            for (int j = 0; j < category.QuestionsViewport.transform.childCount; j++)
            {
                var question = category.QuestionsViewport.transform.GetChild(j).GetComponent<QuestionPrefab>();
        
                if (question.ContentInputField.text.Length <= 1)
                {
                    Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", $"Вы не ввели содержание вопроса {j + 1} категории {i + 1}. Невозможно сохранить файл", 5f);
                    return;
                }
        
                if (question.AnswerInputField.text.Length <= 1)
                {
                    Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", $"Вы не ввели ответ на вопрос {j + 1} категории {i + 1}. Невозможно сохранить файл", 5f);
                    return;
                }
                
                _outputSequence.Questions[category.TitleText.text].Add(new Question(short.Parse(question.CostText.text), question.ContentInputField.text, question.AnswerInputField.text, question.BackgroundInputField.text, question.AudioInputField.text, question.QuestionImageInputField.text, question.AnswerImageInputField.text));
            }
        }
        
        File.WriteAllText(_currentFilePath, JsonConvert.SerializeObject(_outputSequence, Formatting.Indented));
        Toast.Singleton.ShowToast(Toast.ToastMessageType.Success, "Информация", "Файл успешно сохранен.");
        ActionsRequired = false;
    }
    
    internal void LoadFromFile(string filePath)
    {
        if (!GameParser.Singleton.TryDeserializeFromFile(filePath, out Sequence sequence)) return;

        _topicInputField.text = sequence.Topic;
        _descriptionInputField.text = sequence.Description;
        _backgroundInputField.text = sequence.Background;

        foreach (var kvp in sequence.Questions)
        {
            var category = Instantiate(_categoryPrefab, _categoriesViewport.transform).GetComponent<CategoryPrefab>();

            category.TitleText.text = kvp.Key;
            
            foreach (var question in kvp.Value)
            {
                var questionObject = Instantiate(category.QuestionPrefab, category.QuestionsViewport.transform).GetComponent<QuestionPrefab>();

                questionObject.CostText.text = question.Cost.ToString();
                
                questionObject.ContentInputField.text = question.Content;
                questionObject.ContentInputField.onValueChanged.AddListener(delegate { ActionsRequired = true; });
                
                questionObject.AnswerInputField.text = question.Answer;
                questionObject.AnswerInputField.onValueChanged.AddListener(delegate { ActionsRequired = true; });
                
                questionObject.BackgroundInputField.text = question.Background;
                questionObject.BackgroundInputField.onValueChanged.AddListener(delegate { ActionsRequired = true; });
                
                questionObject.QuestionImageInputField.text = question.QuestionImage;
                questionObject.QuestionImageInputField.onValueChanged.AddListener(delegate { ActionsRequired = true; });
                
                questionObject.AnswerImageInputField.text = question.AnswerImage;
                questionObject.AnswerImageInputField.onValueChanged.AddListener(delegate { ActionsRequired = true; });
                
                questionObject.AudioInputField.text = question.Audio;
                questionObject.AudioInputField.onValueChanged.AddListener(delegate { ActionsRequired = true; });
            }
        }

        _currentFilePath = filePath;
        _currentFileText.text = $"Текущий файл: {Path.GetFileNameWithoutExtension(filePath)}";
    }

    internal void ClearFields()
    {
        _topicInputField.text = string.Empty;
        _descriptionInputField.text = string.Empty;
        _backgroundInputField.text = string.Empty;

        foreach (var category in FindObjectsOfType<CategoryPrefab>())
        {
            Destroy(category.gameObject);
        }

        _currentFilePath = string.Empty;
        _currentFileText.text = "Текущий файл: -";
    }
}
