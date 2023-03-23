using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _sequencesViewport;
    [SerializeField] private GameObject _sequencePrefab;
    [SerializeField] private TMP_Text _noFilesWarning;
    
    private List<GameObject> _sequences = new();
    private Sequence _sequenceSelected;
    
    internal string QuestionsFolder = string.Empty;
    internal string GameFilePath = string.Empty;
    
    public static MainMenu Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        if (BuildInfo.Singleton.IsBetaBuild)
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Information, "Информация", "Вы запускаете проект, находящийся на стадии тестирования. Некоторые функции могут работать нестабильно.");
        else if (BuildInfo.Singleton.IsDevelopmentBuild)
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Information, "Информация", "Вы запускаете проект, находящийся на стадии разработки. Весь контент может быть изменен.", 7f);
        
        QuestionsFolder = Path.Combine(Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), "Questions");
        if (!Directory.Exists(QuestionsFolder))
            Directory.CreateDirectory(QuestionsFolder);
    }
    
    public void CheckTemplateExists()
    {
        if (!File.Exists(Path.Combine(QuestionsFolder, "Template.sgamesq")))
            GameParser.Singleton.BuildTemplateFile();
    }

    public void SearchQuestions()
    {
        _noFilesWarning.gameObject.SetActive(Directory.GetFiles(QuestionsFolder).Count(f => Path.GetFileNameWithoutExtension(f) != "Template" && f.EndsWith(".sgamesq")) == 0);
        
        foreach (var file in Directory.GetFiles(QuestionsFolder).Where(f => Path.GetFileNameWithoutExtension(f) != "Template" && f.EndsWith(".sgamesq")))
        {
            if (GameParser.Singleton.TryDeserializeFromFile(file, out Sequence gameSequence))
            {
                var instance = Instantiate(_sequencePrefab, _sequencesViewport.transform).GetComponent<SequencePrefab>();
                
                instance.Title.text = Path.GetFileNameWithoutExtension(file);
                instance.Description.text = gameSequence.Description;
                instance.SelectButton.onClick.AddListener(LoadSequence);
                instance.InfoButton.onClick.AddListener(GetInfo);

                if (!TestConnection.Singleton.IsConnected && GameParser.Singleton.IsUsingNetwork(gameSequence))
                {
                    instance.SelectButton.interactable = false; 
                    Toast.Singleton.ShowToast(Toast.ToastMessageType.Warning, "Игра недоступна", $"Игра \"{gameSequence.Topic}\" недоступна, так как вы не подключены к сети.", 7f);
                }

                _sequences.Add(instance.gameObject);
            }
        }
    }
    
    // Button callback
    public void LoadSequence()
    {
        if (TeamsManager.Singleton.Teams.Count < 2)
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Для начала игры требуется зарегистрировать как минимум две команды.", duration: 5f);
            return;
        }
        
        var filePath = GetSelectedSequencePath();
        
        if (!File.Exists(filePath))
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Указанный вами файл не найден!", 3f);
            return;
        }

        GameFilePath = filePath;
        SceneManager.LoadScene(1);
    }

    // Button callback
    public void GetInfo()
    {
        InfoPanel.Singleton.InfoGui.SetActive(true);
        InfoPanel.Singleton.UpdateInfo();
    }

    public void ClearSequencesList()
    {
        foreach (var sequence in _sequences)
            Destroy(sequence);
        
        _sequences.Clear();
    }
    
    // Button callback
    public void RefreshSequencesList()
    {
        ClearSequencesList();
        SearchQuestions();
    }
    
    // Button callback
    public void OpenInExplorer() => Application.OpenURL($"file://{QuestionsFolder}");

    // Button callback
    public void OpenGithubPage() => Application.OpenURL("https://github.com/bladuk/SGame");

    internal string GetSelectedSequencePath()
    {
        return Path.Combine(Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), "Questions",
            EventSystem.current.currentSelectedGameObject.GetComponentInParent<SequencePrefab>().Title.text + ".sgamesq");
    }
    
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
