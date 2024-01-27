using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class PlayerTickVisual : MonoBehaviour
{
    [SerializeField] private List<Material> _materials;
    [SerializeField] private AnimationCurve _animationCurve;

    [SerializeField] private float _blackFlickerTime = 0.2f;
    private Coroutine _blackFlickerCoroutine;

    private void Reset()
    {
        _materials.Clear();
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            _materials.AddRange(renderer.sharedMaterials);
        }
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<BadTickEvent>(OnBadTick);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<BadTickEvent>(OnBadTick);
    }

    private void OnBadTick(BadTickEvent obj)
    {
        if (_blackFlickerCoroutine != null)
        {
            StopCoroutine(_blackFlickerCoroutine);
        }
        _blackFlickerCoroutine = StartCoroutine(BlackFlicker());
    }

    private IEnumerator BlackFlicker()
    {
        yield return null;
        var startTime = Time.time;
        var endTime = startTime + _blackFlickerTime;
        while (Time.time < endTime)
        {
            var t = (Time.time - startTime) / _blackFlickerTime;
            var colorT = _animationCurve.Evaluate(t);
            Color color = Color.Lerp(Color.white, Color.black, colorT);
            SetMaterialsColor(color);
            yield return null;
        }
        
        SetMaterialsColor(Color.white);

        void SetMaterialsColor(Color color)
        {
            foreach (var material in _materials)
            { 
                material.color = color; 
            }
        }
    }
}