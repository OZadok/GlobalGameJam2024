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

    public ParticleSystem goodParticles;
    
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

    public void ManualPlayerEnter()
    {
        PlayerEnter();
    }

    public void ManualPlayerExit()
    {
        PlayerExit();
    }

    private void PlayerEnter()
    {
        Messenger.Default.Subscribe<PlayerChangedDirectionEvent>(OnPlayerChangedDirection);
        Messenger.Default.Subscribe<TickEvent>(OnTickEvent);
        _waitForClick = true;
        Messenger.Default.Publish(new PlayerEnterNpc(gameObject));
    }
    private void PlayerExit()
    {
        Messenger.Default.Unsubscribe<PlayerChangedDirectionEvent>(OnPlayerChangedDirection);
        Messenger.Default.Unsubscribe<TickEvent>(OnTickEvent);
        Messenger.Default.Publish(new PlayerExitNpc(gameObject));
    }
    
    private void OnPlayerChangedDirection(PlayerChangedDirectionEvent obj)
    {
        var isBefore =  _npcData.NextTimeTick - _forgivenessTimeBefore < Time.time && Time.time < _npcData.NextTimeTick;
        if (_waitForClick && (obj.Direction == _npcData.Direction || isBefore && obj.Direction != _npcData.Direction))
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
        if (tickEvent.NpcData.gameObject != gameObject)
        {
            return;
        }
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
        if (goodParticles != null)
        {
            goodParticles.Play();
        }

        Messenger.Default.Publish(new GoodTickEvent());
    }

    private void BadTick()
    {
        Happiness.ChangeHappiness(gameObject, _badTickHappinessChangePerSecond * _npcData.Period);
        Messenger.Default.Publish(new BadTickEvent());
    }
}
