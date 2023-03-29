using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MEC;

[RequireComponent(typeof(AudioSource))]
public class DynamicAudio : MonoBehaviour
{
    private readonly Dictionary<string, AudioClip> _cache = new();

    public void SetAudio(string url, bool cache = true) => StartCoroutine(DownloadAudio(url, cache));

    private IEnumerator DownloadAudio(string url, bool cache)
    {
        if (_cache.ContainsKey(url))
        {
            gameObject.GetComponent<AudioSource>().clip = _cache[url];
            yield break;
        }

        var request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        yield return request.SendWebRequest();
        Debug.Log(request.downloadProgress.ToString());

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"An error has occured while downloading the audio ({url})\n{request.error}");
            yield break;
        }

        AudioClip webClip = default;

        try
        {
            webClip = ((DownloadHandlerAudioClip) request.downloadHandler).audioClip;

            gameObject.GetComponent<AudioSource>().clip = webClip;
            
            if (cache)
                _cache.Add(url, webClip);
        }
        catch (Exception ex)
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Не удалось получить аудио по указанной ссылке.", duration: 5f);
        }
    }
}
