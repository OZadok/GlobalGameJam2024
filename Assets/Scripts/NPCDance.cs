using System;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class NPCDance : MonoBehaviour
{
    [SerializeField] private float _maxAngle = 45f;
    [SerializeField] private NpcData _npcData;

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
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<TickEvent>(OnTickEvent);
    }

    private void OnTickEvent(TickEvent tickEvent)
    {
        
    }

    private void Update()
    {
        var timeRemain = _npcData.NextTimeTick - Time.time;
        var timeInDirection = Time.time - _npcData.LastTimeTick;
        var ratio = (timeInDirection / _npcData.Period) * 2f - 1f; // from between 0 to 1 to be between -1 to 1
        var angle = _maxAngle * _npcData.Direction * ratio;
        transform.rotation = Quaternion.Euler(0,0, angle);
    }
}