using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] [Multiline] private string _content;

    private void OnDisable()
    {
        Tooltip.Singleton.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Singleton.ShowTooltip(_content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Singleton.HideTooltip();
    }
}
