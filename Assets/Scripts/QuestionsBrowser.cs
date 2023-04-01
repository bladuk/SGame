using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QuestionsBrowser : MonoBehaviour
{
    [SerializeField] private GameObject _sequencesViewport;
    [SerializeField] private GameObject _sequencePrefab;
    [SerializeField] private TMP_Text _noFilesWarning;
    [SerializeField] private GameObject _deleteFileDialog;
    
    private List<GameObject> _sequences = new();
    
    internal string QuestionsFolder { get; private set; } = string.Empty;
    internal string GameFilePath { get; private set; } = string.Empty;
    internal string LastInteractionFile { get; private set; } = string.Empty;
    internal Sequence LoadedSequence { get; private set; }
    
    
    public static QuestionsBrowser Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        QuestionsFolder = Path.Combine(Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), "Questions");
        if (!Directory.Exists(QuestionsFolder))
            Directory.CreateDirectory(QuestionsFolder);
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
                instance.EditButton.onClick.AddListener(EditSequence);
                instance.DeleteButton.onClick.AddListener(DeleteSequence);

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
        GameParser.Singleton.TryDeserializeFromFile(filePath, out Sequence sequence);
        LoadedSequence = sequence;
        SceneManager.LoadScene(1);
    }

    // Button callback
    public void EditSequence()
    {
        EditorScript.Singleton.OpenEditor();
        EditorScript.Singleton.ClearFields();
        EditorScript.Singleton.LoadFromFile(GetSelectedSequencePath());
    }

    // Button callback
    public void DeleteSequence()
    {
        LastInteractionFile = GetSelectedSequencePath();
        _deleteFileDialog.SetActive(true);
    }

    // Button callback
    public void GetInfo()
    {
        InfoPanel.Singleton.PanelBase.SetActive(true);
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
    
    internal string GetSelectedSequencePath()
    {
        return Path.Combine(Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), "Questions",
            EventSystem.current.currentSelectedGameObject.GetComponentInParent<SequencePrefab>().Title.text + ".sgamesq");
    }
}
