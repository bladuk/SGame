using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TeamsWindow : MonoBehaviour
{
    [SerializeField] private GameObject _assurancePanel;

    // Button callback
    public void TryClose()
    {
        if (TeamsManager.Singleton.ActionsRequired)
        {
            _assurancePanel.SetActive(true);
            return;
        }
        
        gameObject.SetActive(false);
    }
}
