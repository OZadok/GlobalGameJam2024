using System;
using System.Collections.Generic;
using Events;
using SuperMaxim.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _travelMusicAudioClips;
    [SerializeField] private AudioSource _travelMusicAudioSource;

    private void Start()
    {
        _travelMusicAudioSource.clip = _travelMusicAudioClips[0];
        _travelMusicAudioSource.Play();
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<WorldHappinessLevelChangedEvent>(OnWorldHappinessLevelChanged);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<WorldHappinessLevelChangedEvent>(OnWorldHappinessLevelChanged);
    }

    private void OnWorldHappinessLevelChanged(WorldHappinessLevelChangedEvent obj)
    {
        Debug.Log("here");
        var index = Mathf.FloorToInt(obj.Level * (_travelMusicAudioClips.Count - 1));
        var targetClip = _travelMusicAudioClips[index];
        if (_travelMusicAudioSource.clip != targetClip)
        {
            _travelMusicAudioSource.clip = targetClip;
            _travelMusicAudioSource.Play();
        }
    }
}
