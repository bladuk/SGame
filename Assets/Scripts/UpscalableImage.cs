using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class UpscalableImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{ 
    [SerializeField] private Texture2D _upscaleCursorIcon;

    private bool _isUpscaled = false;
    private Vector2 _defaultScale;
    private Vector2 _defaultPosition;

    private void Start()
    {
        _defaultPosition = gameObject.GetComponent<RectTransform>().position;
        _defaultScale = gameObject.GetComponent<RectTransform>().sizeDelta;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(_upscaleCursorIcon, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var rectTransform = gameObject.GetComponent<RectTransform>();
        var image = gameObject.GetComponent<Image>();
        if (!_isUpscaled)
        {
            rectTransform.sizeDelta = new Vector2(Screen.width / (image.preferredWidth > image.preferredHeight ? 1.3f : 2.5f), Screen.height - 35f);
            rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        }
        else
        {
            rectTransform.position = _defaultPosition;
            rectTransform.sizeDelta = _defaultScale;
        }

        _isUpscaled = !_isUpscaled;
    }
}
