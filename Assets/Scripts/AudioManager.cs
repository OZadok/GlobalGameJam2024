using System.Collections.Generic;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _travelMusicAudioClips;
    [SerializeField] private AudioSource _travelMusicAudioSource;
    [SerializeField] private AudioSource _fightMusicAudioSource;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerSnapshot _travelSnapshot;
    [SerializeField] private AudioMixerSnapshot _fightSnapshot;
    [SerializeField] private AudioEvent _fightDrumAudioEvent;
    [SerializeField] private AudioEvent _playerChangeDirectionAudioEvent;
    [SerializeField] private AudioEvent _missionSuccessAudioEvent;
    [SerializeField] private AudioEvent _goodTickAudioEvent;
    [SerializeField] private AudioEvent _badTickAudioEvent;

    private GameObject _currentNpc;

    private void Start()
    {
        _travelMusicAudioSource.clip = _travelMusicAudioClips[0];
        _travelMusicAudioSource.Play();
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<WorldHappinessLevelChangedEvent>(OnWorldHappinessLevelChanged);
        Messenger.Default.Subscribe<PlayerEnterNpc>(OnPlayerEnterNpc);
        Messenger.Default.Subscribe<PlayerExitNpc>(OnPlayerExitNpc);
        Messenger.Default.Subscribe<TickEvent>(OnTick);
        Messenger.Default.Subscribe<PlayerChangedDirectionEvent>(OnPlayerChangedDirection);
        Messenger.Default.Subscribe<NpcChangedStateEvent>(OnNpcChangedState);
        Messenger.Default.Subscribe<GoodTickEvent>(OnGoodTick);
        Messenger.Default.Subscribe<BadTickEvent>(OnBadTick);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<WorldHappinessLevelChangedEvent>(OnWorldHappinessLevelChanged);
        Messenger.Default.Unsubscribe<PlayerEnterNpc>(OnPlayerEnterNpc);
        Messenger.Default.Unsubscribe<PlayerExitNpc>(OnPlayerExitNpc);
        Messenger.Default.Unsubscribe<TickEvent>(OnTick);
        Messenger.Default.Unsubscribe<PlayerChangedDirectionEvent>(OnPlayerChangedDirection);
        Messenger.Default.Unsubscribe<NpcChangedStateEvent>(OnNpcChangedState);
        Messenger.Default.Unsubscribe<GoodTickEvent>(OnGoodTick);
        Messenger.Default.Unsubscribe<BadTickEvent>(OnBadTick);
    }

    private void OnBadTick(BadTickEvent obj)
    {
        _badTickAudioEvent.Play();
    }

    private void OnGoodTick(GoodTickEvent obj)
    {
        _goodTickAudioEvent.Play();
    }

    private void OnNpcChangedState(NpcChangedStateEvent obj)
    {
        _missionSuccessAudioEvent.Play();
    }

    private void OnPlayerChangedDirection(PlayerChangedDirectionEvent obj)
    {
        _playerChangeDirectionAudioEvent.Play();
    }

    private void OnTick(TickEvent obj)
    {
        if (_currentNpc != obj.NpcData.gameObject)
        {
            return;
        }

        _fightDrumAudioEvent.Play();
    }

    private void OnPlayerEnterNpc(PlayerEnterNpc obj)
    {
        _currentNpc = obj.NpcGameObject;
        _fightSnapshot.TransitionTo(0.2f);
    }

    private void OnPlayerExitNpc(PlayerExitNpc obj)
    {
        _currentNpc = null;
        _travelSnapshot.TransitionTo(0.2f);
    }

    private void OnWorldHappinessLevelChanged(WorldHappinessLevelChangedEvent obj)
    {
        var index = Mathf.FloorToInt(Mathf.Clamp01(obj.Level) * (_travelMusicAudioClips.Count - 1));
        var targetClip = _travelMusicAudioClips[index];
        if (_travelMusicAudioSource.clip != targetClip)
        {
            _travelMusicAudioSource.clip = targetClip;
            _travelMusicAudioSource.Play();
        }
    }
}