using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Game components")]
    [SerializeField] private List<TMP_Text> _categories;

    [Header("Question components")] 
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _questionTopic;
    [SerializeField] private TMP_Text _question;
    [SerializeField] private TMP_Text _answer; 
    
    [Header("Background image components")]
    [SerializeField] private Image _backgroundImage;

    private const float QuestionPosXDefault = -2.152f;
    private const float QuestionWidthDefault = 1680.223f;
    private const float QuestionPosXImage = -309.247f;
    private const float QuestionWidthImage = 1066.034f;
    
    internal Question CurrentQuestion { get; private set; }
    internal Sequence CurrentSequence { get; private set; }

    public static GameController Singleton;
    
    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        try
        {
            GameParser.Singleton.TryDeserializeFromFile(QuestionsBrowser.Singleton.GameFilePath, out Sequence deserializedSequence);
            
            CurrentSequence = deserializedSequence;
            ResetBackground();
        
            foreach (var category in _categories)
            {
                category.text = deserializedSequence.Questions.Keys.ToList()[_categories.IndexOf(category)];
            }
        }
        catch
        { 
            Quit();
        }
    }

    public void Quit() => SceneManager.LoadScene(0);

    public void Restart() => SceneManager.LoadScene(1);

    public void ProcessButton()
    {
        var button = EventSystem.current.currentSelectedGameObject;
        var category = button.GetComponentInParent<TMP_Text>().text;
        var question = CurrentSequence.Questions[category].FirstOrDefault(q => q.Cost == short.Parse(button.GetComponentInChildren<TMP_Text>().text));
        _questionTopic.text = category;
        _question.text = question.Content;
        _answer.text = question.Answer;
        
        if (question.Background.Length > 1)
            _backgroundImage.GetComponent<DynamicImage>().SetImage(question.Background);
        
        if (question.Image.Length > 0)
        {
            try
            {
                _image.GetComponent<DynamicImage>().SetImage(question.Image);
                _question.rectTransform.sizeDelta = new Vector2(QuestionWidthImage, _question.rectTransform.sizeDelta.y);
                _question.rectTransform.anchoredPosition = new Vector2(QuestionPosXImage, _question.rectTransform.anchoredPosition.y);
            }
            catch
            {
                _image.gameObject.SetActive(false);
                _question.rectTransform.sizeDelta = new Vector2(QuestionWidthDefault, _question.rectTransform.sizeDelta.y);
                _question.rectTransform.anchoredPosition = new Vector2(QuestionPosXDefault, _question.rectTransform.anchoredPosition.y);
            }
        }
        else
        {
            _question.rectTransform.sizeDelta = new Vector2(QuestionWidthDefault, _question.rectTransform.sizeDelta.y);
            _question.rectTransform.anchoredPosition = new Vector2(QuestionPosXDefault, _question.rectTransform.anchoredPosition.y);
        }
        CurrentQuestion = question;
        GameAudio.Singleton.LoadAudio();
        button.SetActive(false);
    }

    public void ResetBackground() => _backgroundImage.GetComponent<DynamicImage>().SetImage(CurrentSequence.Background);
}