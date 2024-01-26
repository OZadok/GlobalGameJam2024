using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NPCFrequency : MonoBehaviour
{
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

    private void Start()
    {
        Tick();
    }

    private void Update()
    {
        if (Time.time >= _npcData.NextTimeTick)
        {
            Tick();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Tick()
    {
        _npcData.LastTimeTick = Time.time;
        _npcData.NextTimeTick = _npcData.LastTimeTick + _npcData.Period;
        _npcData.Direction *= -1f;
        Messenger.Default.Publish(new TickEvent(_npcData));
    }
}
 