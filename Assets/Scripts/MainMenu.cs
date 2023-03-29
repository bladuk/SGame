using UnityEngine;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        if (BuildInfo.Singleton.IsBetaBuild)
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Information, "Информация", "Вы запускаете проект, находящийся на стадии тестирования. Некоторые функции могут работать нестабильно.");
        else if (BuildInfo.Singleton.IsDevelopmentBuild)
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Information, "Информация", "Вы запускаете проект, находящийся на стадии разработки. Весь контент может быть изменен.", 7f);
    }

    // Button callback
    public void OpenGithubPage() => Application.OpenURL("https://github.com/bladuk/SGame");

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
