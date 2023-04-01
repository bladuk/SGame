using UnityEngine;

public class GameEndHandler : MonoBehaviour
{
    internal bool GameEnded = false;

    internal static GameEndHandler Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void OnEnable()
    {
        if (GameObject.FindGameObjectsWithTag("QuestionButton").Length == 0)
        {
            GameEnded = true;
            GameTimer.Singleton.StopTimer();
            Leaderboard.Singleton.Leaderborad.gameObject.SetActive(true);
        }
    }
}
