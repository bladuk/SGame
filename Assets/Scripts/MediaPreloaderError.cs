using System.Linq;
using TMPro;
using UnityEngine;

public class MediaPreloaderError : MonoBehaviour
{
    [SerializeField] private GameObject _errorBase;
    [SerializeField] private TMP_Text _fileName;

    internal static MediaPreloaderError Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    public void ShowErrorMessage()
    {
        _fileName.text = MediaPreloader.Singleton.CurrentFileUrl.Split('/').Last();
        _errorBase.SetActive(true);
    }
}
