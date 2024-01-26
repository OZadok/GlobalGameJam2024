using System.Collections.Generic;
using UnityEngine;

public class NpcAnimationController : MonoBehaviour
{
    [SerializeField] private Transform sadStatesMockRigBaseTransform;
    [SerializeField] private Transform sadStatesParent;
    [SerializeField] private Transform transitionToHappyStatesParent;
    [SerializeField] private Transform happyStatesParent;
    [SerializeField] private float exitSadnessThreshold;

    [Range(0f, 180f)]
    [SerializeField] private float sadRigRotationSpeed = 20f;

    private List<GameObject> _sadStates;
    private float _sadnessStep;


    void Start()
    {
        _sadStates = new List<GameObject>();
        var childCnt = sadStatesParent.childCount;
        _sadnessStep = exitSadnessThreshold / childCnt;
        for (var i = 0; i < childCnt; i++) 
            _sadStates.Add(sadStatesParent.GetChild(i).gameObject);
    }

    public void SetFrequency(float val)
    {
        sadStatesMockRigBaseTransform.localEulerAngles =
            sadStatesMockRigBaseTransform.localEulerAngles.WithZ(val * sadRigRotationSpeed);
    }

    public void SetHappiness(float happiness)
    {
        var targetStateIndex = Mathf.FloorToInt(happiness / _sadnessStep);
        for (var i = 0; i < _sadStates.Count; i++) 
            _sadStates[i].SetActive(i == targetStateIndex);
    }

}
