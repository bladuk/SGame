using System.Collections.Generic;
using System.Linq;
using MEC;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Game components")]
    [SerializeField] private List<TMP_Text> _categories;

    [SerializeField] private Image _image;

    [SerializeField] private TMP_Text _questionTopic;
    [SerializeField] private TMP_Text _question;
    [SerializeField] private TMP_Text _answer;

    [Header("Background image components")]
    [SerializeField] private Image _backgroundImage;

    private const float ContentPosXDefault = -2.152f;
    private const float ContentWidthDefault = 1680.223f;
    private const float ContentPosXImage = -309.247f;
    private const float ContentWidthImage = 1066.034f;

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
            GameParser.Singleton.TryDeserializeFromFile(QuestionsBrowser.Singleton.GameFilePath,
                out Sequence deserializedSequence);

            CurrentSequence = deserializedSequence;
            ResetBackground();

            foreach (var category in _categories)
            {
                category.text = deserializedSequence.Questions.Keys.ToList()[_categories.IndexOf(category)];
            }

            _backgroundImage.GetComponent<DynamicImage>().SetImage(deserializedSequence.Background);
        }
        catch
        {
            Quit();
        }
    }

    public void Quit() => SceneManager.LoadScene(0);

    public void Restart() => SceneManager.LoadScene(2);

    public void ProcessButton()
    {
        var button = EventSystem.current.currentSelectedGameObject;
        var category = button.GetComponentInParent<TMP_Text>().text;
        var question = CurrentSequence.Questions[category]
            .FirstOrDefault(q => q.Cost == short.Parse(button.GetComponentInChildren<TMP_Text>().text));
        _questionTopic.text = category;
        _question.text = question.Content;
        _answer.text = question.Answer;
        
        if (question.Background.Length > 1)
            _backgroundImage.GetComponent<DynamicImage>().SetImage(question.Background);
        
        if (question.QuestionImage.Length > 0)
        {
            SetCanvasImage(question.QuestionImage, _question.rectTransform);
        }
        else
        {
            SetDefaultTransform(_question.rectTransform);
        }
        
        CurrentQuestion = question;
        GameAudio.Singleton.LoadAudio();
        DestroyImmediate(button);
    }

    public void SetAnswerImage()
    {
        if (CurrentQuestion.AnswerImage.Length > 0)
        {
            SetCanvasImage(CurrentQuestion.AnswerImage, _answer.rectTransform);
        }
        else
        {
            SetDefaultTransform(_answer.rectTransform);
        }
    }
    
    public void ResetBackground() => _backgroundImage.GetComponent<DynamicImage>().SetImage(CurrentSequence.Background);
    
    private void SetCanvasImage(string image, RectTransform text)
    {
        try
        {
            _image.GetComponent<DynamicImage>().SetImage(image);
            SetContentTransform(text);
        }
        catch
        {
            _image.gameObject.SetActive(false);
            SetDefaultTransform(text);
        }
    }

    private void SetDefaultTransform(RectTransform rectTransform)
    {
        rectTransform.sizeDelta = new Vector2(ContentWidthDefault, _question.rectTransform.sizeDelta.y);
        rectTransform.anchoredPosition = new Vector2(ContentPosXDefault, _question.rectTransform.anchoredPosition.y);
    }

    private void SetContentTransform(RectTransform rectTransform)
    {
        rectTransform.sizeDelta = new Vector2(ContentWidthImage, _question.rectTransform.sizeDelta.y);
        rectTransform.anchoredPosition = new Vector2(ContentPosXImage, _question.rectTransform.anchoredPosition.y);
    }
}