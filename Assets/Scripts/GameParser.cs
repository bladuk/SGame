using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class GameParser : MonoBehaviour
{
    public static GameParser Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    public void BuildTemplateFile()
    {
        var templateObject = new Sequence("Template", "The template questions file.", "", new Dictionary<string, List<Question>>()
        {
            { "topic1", new List<Question>()
            {
                new Question(100, "content", "answer"), new Question(200, "content", "answer"),
                new Question(300, "content", "answer"), new Question(400, "content", "answer"), 
                new Question(500, "content", "answer")
            }},
            { "topic2", new List<Question>()
            {
                new Question(100, "content", "answer"), new Question(200, "content", "answer"),
                new Question(300, "content", "answer"), new Question(400, "content", "answer"), 
                new Question(500, "content", "answer")
            }},
            { "topic3", new List<Question>()
            {
                new Question(100, "content", "answer"), new Question(200, "content", "answer"),
                new Question(300, "content", "answer"), new Question(400, "content", "answer"), 
                new Question(500, "content", "answer")
            }},
            { "topic4", new List<Question>()
            {
                new Question(100, "content", "answer"), new Question(200, "content", "answer"),
                new Question(300, "content", "answer"), new Question(400, "content", "answer"), 
                new Question(500, "content", "answer")
            }},
            { "topic5", new List<Question>()
            {
                new Question(100, "content", "answer"), new Question(200, "content", "answer"),
                new Question(300, "content", "answer"), new Question(400, "content", "answer"), 
                new Question(500, "content", "answer")
            }},
            { "topic6", new List<Question>()
            {
                new Question(100, "content", "answer"), new Question(200, "content", "answer"),
                new Question(300, "content", "answer"), new Question(400, "content", "answer"), 
                new Question(500, "content", "answer")
            }}
        });
        
        File.WriteAllText(Path.Combine(MainMenu.Singleton.QuestionsFolder, "Template.sgamesq"), JsonConvert.SerializeObject(templateObject, Formatting.Indented));
    }

    public bool TryDeserializeFromFile(string sourcePath, out Sequence gameSequence)
    {
        if (File.Exists(sourcePath))
        {
            try
            {
                gameSequence = JsonConvert.DeserializeObject<Sequence>(File.ReadAllText(sourcePath));
                return gameSequence != null;   
            }
            catch (Exception ex)
            {
                gameSequence = null;
                return false;
            }
        }
        Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Запрашиваемый файл не найден.");
        gameSequence = null;
        return false;
    }

    public bool IsUsingNetwork(Sequence gameSequence) => gameSequence.Background.Length > 0 || gameSequence.Questions.Values.Any(lq => lq.Any(q => q.Background.Length > 0 || q.Image.Length > 0 || q.Audio.Length > 0));
}
