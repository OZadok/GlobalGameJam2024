using System;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class HappyLogo : MonoBehaviour
{
    [SerializeField] private NpcHappinessChanger _npcHappinessChanger;

    private void Awake()
    {
        Reset();
    }

    private void Reset()
    {
        if (_npcHappinessChanger == null)
        {
            _npcHappinessChanger = GetComponent<NpcHappinessChanger>();
        }
    }

    private void Start()
    {
        _npcHappinessChanger.ManualPlayerEnter();
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<GameStartedEvent>(OnGameStarted);
        Messenger.Default.Subscribe<NpcChangedStateEvent>(OnNpcChangedState);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<GameStartedEvent>(OnGameStarted);
        Messenger.Default.Unsubscribe<NpcChangedStateEvent>(OnNpcChangedState);
    }

    private void OnGameStarted(GameStartedEvent obj)
    {
        _npcHappinessChanger.ManualPlayerExit();
    }

    private void OnNpcChangedState(NpcChangedStateEvent obj)
    {
        Messenger.Default.Publish(new RequestStartGameEvent());
    }
}
