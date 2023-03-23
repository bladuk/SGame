using System.Net.Http;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TestConnection : MonoBehaviour
{
    internal bool IsConnected;
    
    public static TestConnection Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        var component = gameObject.GetComponent<TMP_Text>();
        using (HttpClient client = new HttpClient())
        {
            var request = client.GetAsync(PlayerPrefs.GetString("TestServer", "https://google.com/"));
            IsConnected = request.Result.IsSuccessStatusCode;
            component.text = component.text.Replace("%state%", IsConnected ? "установлено." : "не установлено.");
        }
    }
}
