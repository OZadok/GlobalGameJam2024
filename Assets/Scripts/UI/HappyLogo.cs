using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class HappyLogo : MonoBehaviour
{
    [SerializeField] private NpcHappinessChanger _npcHappinessChanger;
    [SerializeField] private NpcData _npcData;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;

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

        if (_npcData == null)
        {
            _npcData = GetComponent<NpcData>();
        }
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }

    private void Start()
    {
        _npcHappinessChanger.ManualPlayerEnter();
        _animator.speed = _npcData.Frequency;
        _rigidbody.simulated = false;
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
        _rigidbody.simulated = true;
    }

    private void OnNpcChangedState(NpcChangedStateEvent obj)
    {
        Messenger.Default.Publish(new RequestStartGameEvent());
    }
}
