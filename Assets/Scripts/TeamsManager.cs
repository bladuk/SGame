using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeamsManager : MonoBehaviour
{
    [SerializeField] private GameObject _teamPrefab;
    [SerializeField] private GameObject _teamsViewport;
    private List<TeamPrefab> _activePrefabs = new();

    internal bool ActionsRequired = false;
    internal List<string> Teams = new();

    public static TeamsManager Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    // Button callback
    public void AddTeam()
    {
        var instance = Instantiate(_teamPrefab, _teamsViewport.transform).GetComponent<TeamPrefab>();
        
        instance.RemoveButton.onClick.AddListener(RemoveTeam);
        _activePrefabs.Add(instance);
        
        UpdateTeamIdentifiers();
        
        if (_activePrefabs.Count >= 2)
            ActionsRequired = true;
    }

    // Button callback
    public void RemoveTeam()
    {
        DestroyImmediate(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        UpdateTeamIdentifiers();
        
        if (_activePrefabs.Count >= 2)
            ActionsRequired = true;
    }
    
    public void UpdateTeamIdentifiers()
    {
        var allTeams = FindObjectsOfType<TeamPrefab>().Reverse().ToArray();
        
        foreach (var team in allTeams)
        {
            team.Id.text = (allTeams.ToList().IndexOf(team) + 1).ToString();
        }
    }

    // Button callback
    public void SaveTeams()
    {
        Teams.Clear();
        
        var allTeams = FindObjectsOfType<TeamPrefab>().Reverse().ToArray();

        if (allTeams.Any(team => team.Name.text.Length <= 1))
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Warning, "Внимание", "Для одной из команд не указано название. Невозможно сохранить список.", duration: 5f);
            return;
        }
        
        foreach (var team in allTeams)
        {
            if (!Teams.Contains(team.Name.text))
                Teams.Add(team.Name.text.TrimStart().TrimEnd());
            else
            {
                Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", $"Вы пытаетесь добавить несколько команд с названием \"{team.Name.text}\". Невозможно сохранить список.", 6f);
                Teams.Clear();
                return;
            }
        }
        
        if (Teams.Count == 0) return;
        
        Toast.Singleton.ShowToast(Toast.ToastMessageType.Success, "Информация", "Список команд успешно сохранен.", duration: 5f);
        ActionsRequired = false;
    }
    
    // Button callback
    public void ClearAllTeams()
    {
        foreach (var team in FindObjectsOfType<TeamPrefab>())
        {
            DestroyImmediate(team.gameObject);
        }
        
        Teams.Clear();
        ActionsRequired = false;
    }
}
