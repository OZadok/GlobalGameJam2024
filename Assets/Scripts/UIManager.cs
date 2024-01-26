using System;
using Events;
using SuperMaxim.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _StartScreen;
    [SerializeField] private GameObject _GameOverScreen;

    private void Start()
    {
        _StartScreen.SetActive(true);
        _GameOverScreen.SetActive(false);
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<GameStartedEvent>(OnGameStarted);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<GameStartedEvent>(OnGameStarted);
    }
    
    public void OnStartGameClicked()
    {
        Messenger.Default.Publish(new RequestStartGameEvent());
    }

    private void OnGameStarted(GameStartedEvent obj)
    {
        _StartScreen.SetActive(false);
        //todo - change to fade animation?
    }
}
