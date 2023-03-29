using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class GameParser : MonoBehaviour
{
    internal readonly Sequence SequenceTemplate = new Sequence("", "", "", new Dictionary<string, List<Question>>()
    {
        {
            "тема 1", new List<Question>()
            {
                new Question(100, "", ""), new Question(200, "", ""),
                new Question(300, "", ""), new Question(400, "", ""),
                new Question(500, "", "")
            }
        },
        {
            "тема 2", new List<Question>()
            {
                new Question(100, "", ""), new Question(200, "", ""),
                new Question(300, "", ""), new Question(400, "", ""),
                new Question(500, "", "")
            }
        },
        {
            "тема 3", new List<Question>()
            {
                new Question(100, "", ""), new Question(200, "", ""),
                new Question(300, "", ""), new Question(400, "", ""),
                new Question(500, "", "")
            }
        },
        {
            "тема 4", new List<Question>()
            {
                new Question(100, "", ""), new Question(200, "", ""),
                new Question(300, "", ""), new Question(400, "", ""),
                new Question(500, "", "")
            }
        },
        {
            "тема 5", new List<Question>()
            {
                new Question(100, "", ""), new Question(200, "", ""),
                new Question(300, "", ""), new Question(400, "", ""),
                new Question(500, "", "")
            }
        },
        {
            "тема 6", new List<Question>()
            {
                new Question(100, "", ""), new Question(200, "", ""),
                new Question(300, "", ""), new Question(400, "", ""),
                new Question(500, "", "")
            }
        }
    });
    
    public static GameParser Singleton;

    private void Awake()
    {
        Singleton = this;
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
