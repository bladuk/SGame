using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ToastPrefab : MonoBehaviour
{
    public CanvasGroup ToastObject;
    public TMP_Text ToastTitle;
    public TMP_Text ToastContent;
    public Image IconImage;
    public Image ProgressBar;
    public Image ToastPanel;
}
