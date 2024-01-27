using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcAnimationController : MonoBehaviour
{
    private readonly int _randomHappyAnimTrigger1 = Animator.StringToHash("Happy1");
    private readonly int _randomHappyAnimTrigger2 = Animator.StringToHash("Happy2");
    private readonly int _randomHappyAnimTrigger3 = Animator.StringToHash("Happy3");
    private readonly int _randomHappyAnimTrigger4 = Animator.StringToHash("Happy4");
    private readonly int _randomHappyAnimTrigger5 = Animator.StringToHash("Happy5");
    private readonly int _randomHappyAnimTrigger6 = Animator.StringToHash("Happy6");
    private readonly int _randomHappyAnimTrigger7 = Animator.StringToHash("Happy7");
    private readonly int _randomHappyAnimTrigger8 = Animator.StringToHash("Happy8");
    private readonly int _randomHappyAnimTrigger9 = Animator.StringToHash("Happy9");
    private int[] _randomHappyAnimTriggers;

    [SerializeField] private Transform sadStatesMockRigBaseTransform;
    [SerializeField] private Transform sadStatesParent;
    [SerializeField] private Transform transitionToHappyStatesParent;
    [SerializeField] private Animator happyAnimator;

    [Range(0f, 180f)]
    [SerializeField] private float sadRigRotationSpeed = 20f;
    
    [Range(0f, 30f)]
    [SerializeField] private float transitionToHappyFrameRate = 6f;
    
    [Range(0f, 3f)]
    [SerializeField] private float happinessTweenDuration = 0.33f;

    private List<GameObject> _sadStates;
    private List<GameObject> _transitionToHappyStates;
    private float _sadnessStep;
    private float _currHappiness;
    private Tween _happinessTween;


    void Start()
    {
        _sadStates = new List<GameObject>();
        var childCnt = sadStatesParent.childCount;
        for (var i = 0; i < childCnt; i++) 
            _sadStates.Add(sadStatesParent.GetChild(i).gameObject);
        
        _transitionToHappyStates = new List<GameObject>();
        var transitionChildCnt = transitionToHappyStatesParent.childCount;
        for (var i = 0; i < transitionChildCnt; i++) 
            _transitionToHappyStates.Add(transitionToHappyStatesParent.GetChild(i).gameObject);
        
        _randomHappyAnimTriggers = new[]
        {
            _randomHappyAnimTrigger1, _randomHappyAnimTrigger2, _randomHappyAnimTrigger3, 
            _randomHappyAnimTrigger4, _randomHappyAnimTrigger5, _randomHappyAnimTrigger6, 
            _randomHappyAnimTrigger7, _randomHappyAnimTrigger8, _randomHappyAnimTrigger9
        };
    }

    public void SetFrequency(float val)
    {
        sadStatesMockRigBaseTransform.localEulerAngles =
            sadStatesMockRigBaseTransform.localEulerAngles.WithZ(val * sadRigRotationSpeed);
    }
    
    public void SetHappiness(float happiness)
    {
        _happinessTween?.Kill();
        _happinessTween = DOVirtual.Float(_currHappiness, happiness, happinessTweenDuration, (val) =>
        {
            _currHappiness = val; 
            var targetStateIndex = Mathf.FloorToInt(_currHappiness * (_sadStates.Count - 1));
            for (var i = 0; i < _sadStates.Count; i++) 
                _sadStates[i].SetActive(i == targetStateIndex);
        });
    }

    public void BecomeHappy()
    {
        StartCoroutine(TransitionToHappy());
    }

    private IEnumerator TransitionToHappy()
    {
        transitionToHappyStatesParent.gameObject.SetActive(true); // Assuming all children are not active 
        foreach (var go in _transitionToHappyStates)
        {
            go.SetActive(true); 
            yield return new WaitForSeconds(1 / transitionToHappyFrameRate);
            go.SetActive(false);
        }

        sadStatesParent.gameObject.SetActive(false);
        transitionToHappyStatesParent.gameObject.SetActive(false);
        happyAnimator.gameObject.SetActive(true);
        
        var trigger = _randomHappyAnimTriggers[Random.Range(0,_randomHappyAnimTriggers.Length)];
        happyAnimator.SetTrigger(trigger);
    }

}
