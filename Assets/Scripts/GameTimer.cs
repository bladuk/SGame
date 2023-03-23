using System;
using System.Collections.Generic;
using MEC;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timer;
    private DateTime _gameStartedTime;
    private CoroutineHandle _timerCoroutine;

    private void Start()
    {
        _gameStartedTime = DateTime.Now;
        _timerCoroutine = Timing.RunCoroutine(_updateTimer());
    }

    public void StopTimer() => Timing.KillCoroutines(_timerCoroutine);

    private IEnumerator<float> _updateTimer()
    {
        for (;;)
        {
            yield return Timing.WaitForSeconds(1f);
            _timer.text = DateTime.Now.Subtract(_gameStartedTime).ToString().Split('.')[0];
        }
    }
}
