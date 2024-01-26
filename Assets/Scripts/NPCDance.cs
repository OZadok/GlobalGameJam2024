using System;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class NPCDance : MonoBehaviour
{
    [SerializeField] private float _maxAngle = 45f;
    [SerializeField] private NpcData _npcData;
    [SerializeField] private NpcAnimationController _npcAnimationController;

    private void Awake()
    {
        Reset();
    }

    private void Reset()
    {
        if (_npcData == null)
        {
            _npcData = GetComponent<NpcData>();
        }
    }
    private void OnEnable()
    {
        Messenger.Default.Subscribe<TickEvent>(OnTickEvent);
        Messenger.Default.Subscribe<HappinessChangedEvent>(OnHappinessChangedEvent);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<TickEvent>(OnTickEvent);
        Messenger.Default.Unsubscribe<HappinessChangedEvent>(OnHappinessChangedEvent);
    }

    private void OnTickEvent(TickEvent tickEvent)
    {
        
    }

    
    private void OnHappinessChangedEvent(HappinessChangedEvent obj)
    {
        _npcAnimationController.SetHappiness(obj.Level);
    }

    [Range(-1,1)] public float test;
    private void Update()
    {
        var timeRemain = _npcData.NextTimeTick - Time.time;
        var timeInDirection = Time.time - _npcData.LastTimeTick;
        var ratio = (timeInDirection / _npcData.Period) * 2f - 1f; // from between 0 to 1 to be between -1 to 1
        // _npcAnimationController.SetFrequency(_npcData.Direction * ratio);
        test = _npcData.Direction * ratio;
        _npcAnimationController.SetFrequency(test);
        
        var angle = _maxAngle * _npcData.Direction * ratio * -1f;
        transform.rotation = Quaternion.Euler(0,0, angle);
    }
}