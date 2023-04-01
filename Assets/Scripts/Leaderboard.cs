using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] internal GameObject Leaderborad;
    
    [Header("Teams")]
    [SerializeField] private GameObject _teamPrefab;
    [SerializeField] private GameObject _teamsViewport;
    [SerializeField] private ContentSizeFitter _teamsContentSizeFitter;

    [Header("Scores")] 
    [SerializeField] private GameObject _scorePrefab;
    [SerializeField] private GameObject _scoresViewport;
    [SerializeField] private ContentSizeFitter _scoreContentSizeFitter;

    public static Leaderboard Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        foreach (var team in TeamsManager.Singleton.Teams)
        {
            Instantiate(_teamPrefab, _teamsViewport.transform).GetComponent<TMP_Text>().text = $"{TeamsManager.Singleton.Teams.IndexOf(team) + 1}. {team}";
            Instantiate(_scorePrefab, _scoresViewport.transform).GetComponent<TMP_Text>().text = "0";
        }
    }

    private void Update()
    {
        if (!GameEndHandler.Singleton.GameEnded)
        {
            _teamsContentSizeFitter.enabled = TeamsManager.Singleton.Teams.Count > 9;
            _scoreContentSizeFitter.enabled = TeamsManager.Singleton.Teams.Count > 9;
        
            Leaderborad.SetActive(Input.GetKey(KeyCode.Tab));   
        }
    }

    public void UpdateLeaderboardScore(int teamId)
    {
        _scoresViewport.transform.GetChild(teamId).GetComponent<TMP_Text>().text = ScoresManager.Singleton.TeamScores.Values.ElementAt(teamId).ToString();
    }
}
