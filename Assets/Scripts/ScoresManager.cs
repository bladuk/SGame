using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoresManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _teamsDropdown;

    public static ScoresManager Singleton;
    
    internal readonly Dictionary<string, int> TeamScores = new();

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        foreach (var team in TeamsManager.Singleton.Teams)
        {
            TeamScores.Add(team, 0);
            
            _teamsDropdown.options.Add(new TMP_Dropdown.OptionData($"{TeamsManager.Singleton.Teams.IndexOf(team) + 1}. {team}"));
        }
    }

    // Button callback
    public void AddScore()
    {
        if (TryAddScore(_teamsDropdown.options[_teamsDropdown.value].text.Remove(0, 3), GameController.Singleton.CurrentQuestion.Cost))
        {
            Leaderboard.Singleton.UpdateLeaderboardScore(_teamsDropdown.value);
        }
        else
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Не удалось добавить очки команде.", 5f);
        }
    }

    // Button callback
    public void TakeScore()
    {
        if (TryAddScore(_teamsDropdown.options[_teamsDropdown.value].text.Remove(0, 3), GameController.Singleton.CurrentQuestion.Cost * -1))
        {
            Leaderboard.Singleton.UpdateLeaderboardScore(_teamsDropdown.value);   
        }
        else
        {
            Toast.Singleton.ShowToast(Toast.ToastMessageType.Error, "Ошибка", "Не удалось снять очки команде.", 5f);
        }
    }
    
    private bool TryAddScore(string team, int score)
    {
        if (!TeamScores.ContainsKey(team))
        {
            Debug.LogError($"Team {team} doesn't exists!");
            return false;
        }

        TeamScores[team] += score;
        return true;
    }
}
