using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.Assertions;

public class NpcHappinessChanger : MonoBehaviour
{
    [SerializeField] private NpcData _npcData;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float _forgivenessTimeBefore = 0.1f;
    [SerializeField] private float _forgivenessTimeAfter = 0.1f;

    [SerializeField] private float _goodTickHappinessChangePerSecond;
    [SerializeField] private float _badTickHappinessChangePerSecond;

    private bool _waitForClick;
    
    private Coroutine waitAfterCoroutine;
    private Coroutine waitBeforeCoroutine;
    
    private void Awake()
    {
        Reset();
    }

    private void Reset()
    {
        if(_npcData == null)
            _npcData = GetComponent<NpcData>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<PlayerChangedDirectionEvent>(OnPlayerChangedDirection);
        Messenger.Default.Unsubscribe<TickEvent>(OnTickEvent);
        
        if (waitAfterCoroutine != null)
        {
            StopCoroutine(waitAfterCoroutine);
        }
        if (waitBeforeCoroutine != null)
        {
            StopCoroutine(waitBeforeCoroutine);
        }
        
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((layerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            PlayerEnter();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if ((layerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            PlayerExit();
        }
    }

    private void PlayerEnter()
    {
        Messenger.Default.Subscribe<PlayerChangedDirectionEvent>(OnPlayerChangedDirection);
        Messenger.Default.Subscribe<TickEvent>(OnTickEvent);
    }
    private void PlayerExit()
    {
        Messenger.Default.Unsubscribe<PlayerChangedDirectionEvent>(OnPlayerChangedDirection);
        Messenger.Default.Unsubscribe<TickEvent>(OnTickEvent);
    }
    
    private void OnPlayerChangedDirection(PlayerChangedDirectionEvent obj)
    {
        if (_waitForClick && obj.Direction == _npcData.Direction)
        {
            _waitForClick = false;
            GoodTick();
        }
        else
        {
            BadTick();
        }
    }
    
    private void OnTickEvent(TickEvent tickEvent)
    {
        if (waitAfterCoroutine != null)
        {
            StopCoroutine(waitAfterCoroutine);
        }
        if (waitBeforeCoroutine != null)
        {
            StopCoroutine(waitBeforeCoroutine);
        }
        var timeToNotWaitForTick = Time.time + _forgivenessTimeAfter;
        var timeToWaitForTick = _npcData.NextTimeTick - _forgivenessTimeBefore;
        
        var toChangeWaitForTickToFalse = _waitForClick && (timeToWaitForTick > timeToNotWaitForTick);
        if (toChangeWaitForTickToFalse)
        {
            waitAfterCoroutine = StartCoroutine(SetWaitForClickAfterForgiveness());
        }
        
        waitBeforeCoroutine = StartCoroutine(SetWaitForClickBeforeForgiveness(timeToWaitForTick));
    }

    private IEnumerator SetWaitForClickAfterForgiveness()
    {
        yield return new WaitForSeconds(_forgivenessTimeAfter);
        if (_waitForClick)
        {
            BadTick();
        }
        _waitForClick = false;
    }
    
    private IEnumerator SetWaitForClickBeforeForgiveness(float nextWaitTime)
    {
        yield return new WaitForSeconds(nextWaitTime - Time.time);
        _waitForClick = true;
    }

    private void GoodTick()
    {
        Happiness.ChangeHappiness(gameObject, _goodTickHappinessChangePerSecond * _npcData.Period);
    }

    private void BadTick()
    {
        Happiness.ChangeHappiness(gameObject, _badTickHappinessChangePerSecond * _npcData.Period);
    }
}
