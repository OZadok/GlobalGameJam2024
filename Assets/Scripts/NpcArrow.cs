using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.Serialization;

public class NpcArrow : MonoBehaviour
{
    [SerializeField] private NpcData _npcData;
    [SerializeField] private GameObject NpcGameObject;
    [SerializeField] private GameObject _arrowPivotRotation;
    [SerializeField] private GameObject _arrowPivotPosition;

    private Tween _baseAnimationTween;

    private void Start()
    {
        _arrowPivotRotation.SetActive(false);
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<PlayerEnterNpc>(OnPlayerEnter);
        Messenger.Default.Subscribe<PlayerExitNpc>(OnPlayerExit);
        Messenger.Default.Subscribe<TickEvent>(OnTick);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<PlayerEnterNpc>(OnPlayerEnter);
        Messenger.Default.Unsubscribe<PlayerExitNpc>(OnPlayerExit);
        Messenger.Default.Unsubscribe<TickEvent>(OnTick);
    }

    private void OnPlayerEnter(PlayerEnterNpc obj)
    {
        if (obj.NpcGameObject != NpcGameObject)
        {
            return;
        }

        ShowArrow();
    }

    private void OnPlayerExit(PlayerExitNpc obj)
    {
        if (obj.NpcGameObject != NpcGameObject)
        {
            return;
        }

        HideArrow();
    }

    private void ShowArrow()
    {
        _arrowPivotRotation.SetActive(true);
        _arrowPivotPosition.transform.DOLocalMoveY(7, 0.5f).SetEase(Ease.OutBack);//.OnComplete(ArrowBaseAnimation);
    }

    private void HideArrow()
    {
        //SmoothKillTween(_baseAnimationTween);
        _arrowPivotPosition.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.InBack).OnComplete(() => _arrowPivotRotation.SetActive(false));
    }

    private void OnTick(TickEvent obj)
    {
        if (obj.NpcData != _npcData)
        {
            return;
        }

        var angle = (obj.NpcData.Direction - 1) * 90;
        //SmoothKillTween(_baseAnimationTween);
        //_arrowPivotPosition.transform.DOLocalMoveX(2 * _npcData.Direction, 0);
        _arrowPivotRotation.transform.DOLocalRotate(new Vector3(0, angle, 0), 0.15f).OnComplete(ArrowBaseAnimation);
    }

    private void ArrowBaseAnimation()
    {
        _arrowPivotPosition.transform.DOLocalMoveX(2 * _npcData.Direction, 0);
        _baseAnimationTween =
            _arrowPivotPosition.transform.DOLocalMoveX(-2 * _npcData.Direction,
                _npcData.Period - 0.15f); //.SetLoops(-1);
    }

    private static void SmoothKillTween(Tween tween)
    {
        tween.SetLoops(0);
        tween.Complete();
        tween.Kill();
    }
}