using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerColliderStayHappinessChanger : MonoBehaviour
{
    [SerializeField] private float _amountPerSecond;

    private void OnTriggerStay2D(Collider2D other)
    {
        Happiness.ChangeHappiness(other.gameObject, _amountPerSecond * Time.fixedDeltaTime);
    }
}