using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class EditorWindow : MonoBehaviour
{
    [SerializeField] private GameObject _assurancePanel;

    // Button callback
    public void TryClose()
    {
        if (EditorScript.Singleton.ActionsRequired)
        {
            _assurancePanel.SetActive(true);
            return;
        }
        
        gameObject.SetActive(false);
    }
}
