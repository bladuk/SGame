using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text _tooltipContent;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private int _contentCharacterWrapLimit;

    public static Tooltip Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        HideTooltip();
    }

    private void Update()
    {
        Vector2 position = Input.mousePosition;
        
        _layoutElement.enabled = _tooltipContent.text.Length > _contentCharacterWrapLimit;
        _rectTransform.pivot = new Vector2(position.x / Screen.width, position.y / Screen.height);
        
        transform.position = position;
    }

    public void ShowTooltip(string content)
    {
        gameObject.SetActive(true);
        _tooltipContent.text = content;
    }

    public void HideTooltip()
    {
        _tooltipContent.text = string.Empty;
        gameObject.SetActive(false);
    }
}
