using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MediaPreloader : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private TMP_Text _fileSize;
    [SerializeField] private TMP_Text _filesCount;

    private CoroutineHandle _downloadCoroutine;

    internal readonly Dictionary<string, Sprite> SpritesCache = new();
    internal readonly Dictionary<string, AudioClip> AudiosCache = new();

    internal static MediaPreloader Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        List<string> imageQueue = new();
        List<string> audioQueue = new();
        
        if (QuestionsBrowser.Singleton.LoadedSequence.Background.Length > 0)
            imageQueue.Add(QuestionsBrowser.Singleton.LoadedSequence.Background);
        
        foreach (var question in QuestionsBrowser.Singleton.LoadedSequence.Questions.Values.SelectMany(q => q))
        {
            if (!string.IsNullOrEmpty(question.Image) && !imageQueue.Contains(question.Image))
                imageQueue.Add(question.Image);
            
            if (!string.IsNullOrEmpty(question.Background) && !imageQueue.Contains(question.Background))
                imageQueue.Add(question.Background);
            
            if (!string.IsNullOrEmpty(question.Audio) && !imageQueue.Contains(question.Audio))
                audioQueue.Add(question.Audio);
        }

        _downloadCoroutine = Timing.RunCoroutine(DownloadContent(imageQueue, audioQueue));
    }
    
    private IEnumerator<float> DownloadContent(List<string> imageUrls, List<string> audioUrls)
    {
        foreach (var url in imageUrls)
        {
            _filesCount.text = $"{imageUrls.IndexOf(url) + 1} / {imageUrls.Count}";
            var request = UnityWebRequestTexture.GetTexture(url);
            request.SendWebRequest();

            while (!request.isDone)
            {
                yield return Timing.WaitForOneFrame;
                _progressBar.fillAmount = request.downloadProgress;

                float.TryParse(request.GetResponseHeader("Content-Length"), out float totalBytes);
            
                _fileSize.text = $"{Math.Round((decimal)((float)request.downloadedBytes / 1048576), 2)} Mb / {Math.Round((decimal)(totalBytes / 1048576), 2)} Mb";
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
                throw new Exception($"An error has occured while downloading the image ({url})\n{request.error}");
            }
        
            Sprite webSprite = SpriteFromTexture2D(((DownloadHandlerTexture)request.downloadHandler).texture);

            SpritesCache.Add(url, webSprite);
        }

        foreach (var url in audioUrls)
        {
            var request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            request.SendWebRequest();
            
            _filesCount.text = $"{audioUrls.IndexOf(url) + 1} / {audioUrls.Count}";
            
            while (!request.isDone)
            {
                yield return Timing.WaitForOneFrame;
                _progressBar.fillAmount = request.downloadProgress;

                float.TryParse(request.GetResponseHeader("Content-Length"), out float totalBytes);
            
                _fileSize.text = $"{Math.Round((decimal)((float)request.downloadedBytes / 1048576), 2)} Mb / {Math.Round((decimal)(totalBytes / 1048576), 2)} Mb";
            }
            
            if (request.result != UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene(0);
                throw new Exception($"An error has occured while downloading the image ({url})\n{request.error}");
            }
            
            AudioClip webClip = ((DownloadHandlerAudioClip) request.downloadHandler).audioClip;
            
            AudiosCache.Add(url, webClip);
        }

        Debug.Log("All content is downloaded!");
        SceneManager.LoadScene(2);
    }

    private Sprite SpriteFromTexture2D(Texture2D texture) => Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    public void CancelDownloading()
    {
        Timing.KillCoroutines(_downloadCoroutine);
        SceneManager.LoadScene(0);
    }
}
