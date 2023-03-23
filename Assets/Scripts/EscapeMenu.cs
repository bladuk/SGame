using TMPro;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private TMP_Text _menuWarning;
    [SerializeField] private TMP_Text _gameTitle;

    private void Start() => _gameTitle.text = GameController.Singleton.CurrentSequence.Topic;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) || _menu == null || _menuWarning == null) return;
        
        _menu.SetActive(!_menu.activeSelf);
        _menuWarning.gameObject.SetActive(!_menuWarning.gameObject.activeSelf);
    }
}
