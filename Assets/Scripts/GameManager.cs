using System.Collections.Generic;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float _worldHappinessLevel;
    private int _npcsAmount => _npcs.Count;
    private List<NpcData> _npcs = new List<NpcData>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddNpcData(NpcData npcData)
    {
        _npcs.Add(npcData);
    }


    private void Start()
    {
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<RequestStartGameEvent>(StartGame);
        Messenger.Default.Subscribe<NpcChangedStateEvent>(OnNpcChangedState);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<RequestStartGameEvent>(StartGame);
        Messenger.Default.Unsubscribe<NpcChangedStateEvent>(OnNpcChangedState);
    }

    private void StartGame(RequestStartGameEvent obj)
    {
        Messenger.Default.Publish(new GameStartedEvent());
    }

    private void OnNpcChangedState(NpcChangedStateEvent obj)
    {
        if (_npcsAmount <= 1)
        {
            _worldHappinessLevel = 1;
        }
        else
        {
            _worldHappinessLevel += 1f / (_npcsAmount - 1);
        }

        Messenger.Default.Publish(new WorldHappinessLevelChangedEvent(_worldHappinessLevel));
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}