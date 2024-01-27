using System;
using UnityEngine;

public class NpcData : MonoBehaviour
{
    public float Frequency;
    public float Period => 1f / Frequency;
    [HideInInspector] public float Direction = 1f;
    public float LastTimeTick { get; set; }
    public float NextTimeTick { get; set; }

    private void Start()
    {
        GameManager.Instance.AddNpcData(this);
    }
}
