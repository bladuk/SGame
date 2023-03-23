using System.Linq;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] internal GameObject InfoGui;
    [SerializeField] private TMP_Text _infoTitle;
    [SerializeField] private TMP_Text _infoContent;
    public static InfoPanel Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    public void UpdateInfo()
    {
        string questionsString = string.Empty;
        GameParser.Singleton.TryDeserializeFromFile(MainMenu.Singleton.GetSelectedSequencePath(), out Sequence selectedSequence);

        foreach (var topic in selectedSequence.Questions.Keys)
            questionsString += $"Тема {selectedSequence.Questions.Keys.ToList().IndexOf(topic) + 1}: {topic}.\n";
        
        _infoTitle.text = selectedSequence.Topic;
        _infoContent.text = $"Описание: {selectedSequence.Description}\n\nТемы вопросов:\n{questionsString}";
    }
}
