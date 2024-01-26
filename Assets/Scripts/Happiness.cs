using System;
using SuperMaxim.Messaging;
using UnityEngine;

public class Happiness : MonoBehaviour
{
    [SerializeField] [Range(0,1)] private float _level;

    private void OnEnable()
    {
        Messenger.Default.Subscribe<RequestHappinessChangeEvent>(OnHappinessChange);
    }
    
    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<RequestHappinessChangeEvent>(OnHappinessChange);
    }

    private void OnHappinessChange(RequestHappinessChangeEvent obj)
    {
        if (obj.GameObject != gameObject)
        {
            return;
        }

        _level += obj.Amount;

        Messenger.Default.Publish(new HappinessChangedEvent(gameObject, _level));
    }

    public static void ChangeHappiness(GameObject to, float amount)
    {
        Messenger.Default.Publish(new RequestHappinessChangeEvent(to, amount));
    }
}
