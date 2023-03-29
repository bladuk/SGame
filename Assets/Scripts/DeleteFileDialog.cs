using System;
using System.IO;
using UnityEngine;

public class DeleteFileDialog : MonoBehaviour
{
    public void DeleteFile()
    {
        try
        {
            File.Delete(QuestionsBrowser.Singleton.LastInteractionFile);
            QuestionsBrowser.Singleton.RefreshSequencesList();
        }
        catch (Exception ex)
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Не удалось удалить файл.");
            Debug.LogException(ex);
        }
    }
}
