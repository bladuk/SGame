using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WindowsController : MonoBehaviour
{
    private List<GameObject> _activeWindows = new();

    private void OnDisable()
    {
        _activeWindows.Clear();
    }

    private void Start()
    {
        foreach (var button in Resources.FindObjectsOfTypeAll<Button>())
            button.onClick.AddListener(UpdateActiveWindowsList);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _activeWindows.Count > 0)
        {
            var window = _activeWindows[^1];
            
            window.SetActive(false);
            _activeWindows.Remove(window);
        }
    }

    private void UpdateActiveWindowsList()
    {
        Debug.Log("Updating active windows list");
        foreach (var window in _activeWindows.Where(window => !window.activeSelf))
        {
            _activeWindows.Remove(window);
        }

        foreach (var item in GameObject.FindGameObjectsWithTag("Window"))
        {
            if (item.activeSelf && !_activeWindows.Contains(item))
                _activeWindows.Add(item);
        }
    }
}
