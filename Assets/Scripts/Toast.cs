using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MEC;
using UnityEngine.EventSystems;

public class Toast : MonoBehaviour
{
    [SerializeField] private GameObject _toastPrefab;
    [SerializeField] private GameObject _toastsViewport;
    [SerializeField] private List<Sprite> _icons;
    private List<GameObject> _allToasts = new();

    public static Toast Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private bool IsOnToastObject()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        
        foreach (var toast in _allToasts)
        {
            if (results.Any(hit => hit.gameObject == toast.GetComponent<ToastPrefab>().ToastPanel.gameObject))
                return true;
        }

        return false;
    }

    public void ShowToast(ToastMessageType type, string title, string content, float duration = 3f, float transition = 1f)
    {
        Debug.Log($"{title} -- {content} [duration: {duration}, transition: {transition}]");
        Timing.RunCoroutine(ToastDisplay(type, title, content, duration, transition), "Toast");
    }

    private IEnumerator<float> ToastDisplay(ToastMessageType type, string title, string content, float duration, float transition = 1f)
    {
        var toast = Instantiate(_toastPrefab, _toastsViewport.transform).GetComponent<ToastPrefab>();
        
        _allToasts.Add(toast.gameObject);
        
        toast.IconImage.sprite = _icons[(int) type];
        toast.ToastTitle.text = title;
        toast.ToastContent.text = content;
        toast.ProgressBar.fillAmount = 1f;
        while (toast.ToastObject.alpha < 1f)
        {
            yield return Timing.WaitForOneFrame;
            toast.ToastObject.alpha += Time.deltaTime / transition;
        }
        
        while (toast.ProgressBar.fillAmount > 0f)
        {
            yield return Timing.WaitForOneFrame;
            if (!IsOnToastObject())
                toast.ProgressBar.fillAmount -= Time.deltaTime / duration;
        }
        
        while (toast.ToastObject.alpha > 0f)
        {
            yield return Timing.WaitForOneFrame;
            toast.ToastObject.alpha -= Time.deltaTime / transition;
        }

        _allToasts.Remove(toast.gameObject);
        DestroyImmediate(toast.gameObject);
    }

    public enum ToastMessageType : byte
    {
        Information,
        Success,
        Warning,
        Error
    }
}
