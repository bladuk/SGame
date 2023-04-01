using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DynamicAudio : MonoBehaviour
{
    public void SetAudio(string url)
    {
        if (MediaPreloader.Singleton.AudiosCache.ContainsKey(url))
        {
            gameObject.GetComponent<AudioSource>().clip = MediaPreloader.Singleton.AudiosCache[url];
        }
    }
}
