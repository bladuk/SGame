using System;
using MEC;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class GameAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _primaryAudioSource;
    [SerializeField] private GameObject _audioObjectsGroup;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private TMP_Text _volumeValue;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _stopButton;

    private CoroutineHandle _stopCoroutine;

    public static GameAudio Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_primaryAudioSource.isPlaying)
                PlayAudio();
            else
                StopAudio();
        }
            
    }

    public void LoadAudio()
    {
        if (GameController.Singleton.CurrentQuestion.Audio.Length <= 0) return;
        try
        {
            _primaryAudioSource.GetComponent<DynamicAudio>().SetAudio(GameController.Singleton.CurrentQuestion.Audio);
            _audioObjectsGroup.SetActive(true);
        }
        catch
        {
            _audioObjectsGroup.SetActive(false);
        }
    }

    public void PlayAudio()
    {
        if (_primaryAudioSource.clip == null) return;
        
        _primaryAudioSource.loop = false;
        _primaryAudioSource.Play();

        _stopCoroutine = Timing.CallDelayed(_primaryAudioSource.clip.length, delegate()
        {
            StopAudio();
            _playButton.gameObject.SetActive(true);
            _stopButton.gameObject.SetActive(false);
        });
    }

    public void StopAudio()
    {
        if (_primaryAudioSource.clip == null) return;

        if (_stopCoroutine != null)
            Timing.KillCoroutines(_stopCoroutine);
        
        _primaryAudioSource.Stop();
    }

    public void UpdateValue()
    {
        _primaryAudioSource.volume = _volumeSlider.value;
        _volumeValue.text = Math.Floor(_volumeSlider.value * 100) + "%";
    }
}
