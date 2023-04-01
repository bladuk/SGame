using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DynamicImage : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSprite;
    
    public void SetImage(string url)
    {
        gameObject.GetComponent<Image>().sprite = MediaPreloader.Singleton.SpritesCache.ContainsKey(url) ? MediaPreloader.Singleton.SpritesCache[url] : _defaultSprite;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }
}
