using System.Collections.Generic;
using Cinemachine;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class StartGameHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private List<GameObject> _gameObjectsToRemove;
    private void Start()
    {
        //lock camera
        _cinemachineVirtualCamera.enabled = false;
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<GameStartedEvent>(OnGameStarted);
    }
    
    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<GameStartedEvent>(OnGameStarted);
    }
    private void OnGameStarted(GameStartedEvent obj)
    {
        //free camera
        _cinemachineVirtualCamera.enabled = true;
        //remove colliders
        foreach (var gameObjectToRemove in _gameObjectsToRemove)
        {
            gameObjectToRemove.SetActive(false);
        }
    }
}
