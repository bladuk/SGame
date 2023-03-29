using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DynamicImage : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSprite;

    private readonly Dictionary<string, Sprite> _cache = new();

    public void SetImage(string url, bool cache = true)
    {
        Debug.Log(url.Length.ToString());
        if (url.Length <= 1)
        {
            gameObject.GetComponent<Image>().sprite = _defaultSprite;
            return;
        }
        
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        StartCoroutine(DownloadImage(url, cache));
    }

    private IEnumerator DownloadImage(string url, bool cache)
    {
        if (_cache.ContainsKey(url))
        {
            gameObject.GetComponent<Image>().sprite = _cache[url];
            yield break;
        }

        var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        Debug.Log(request.downloadProgress.ToString());
        
        if (request.result != UnityWebRequest.Result.Success)
        {
            if (_defaultSprite != null)
                gameObject.GetComponent<Image>().sprite = _defaultSprite;
            else 
                gameObject.SetActive(false);
            
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Не удалось получить изображение по указанной ссылке.", duration: 5f);
            throw new Exception($"An error has occured while downloading the image ({url})\n{request.error}");
        }
        
        Sprite webSprite = SpriteFromTexture2D(((DownloadHandlerTexture)request.downloadHandler).texture);
        gameObject.GetComponent<Image>().sprite = webSprite;
        
        if (cache)
            _cache.Add(url, webSprite);
    }
	
    private Sprite SpriteFromTexture2D(Texture2D texture) => Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
}
