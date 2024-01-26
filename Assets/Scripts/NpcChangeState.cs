using System.Collections.Generic;
using SuperMaxim.Messaging;
using UnityEngine;

public class NpcChangeState : MonoBehaviour
{
    [SerializeField] private List<Component> _componnentsToRemove;
    private void OnEnable()
    {
        Messenger.Default.Subscribe<HappinessChangedEvent>(OnHappinessChanged);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<HappinessChangedEvent>(OnHappinessChanged);
    }

    private void OnHappinessChanged(HappinessChangedEvent happinessChangedEvent)
    {
        if (happinessChangedEvent.GameObject != gameObject)
        {
            return;
        }

        if (happinessChangedEvent.Level < 1f)
        {
            return;
        }

        ChangeStateToLaugh();
    }

    private void ChangeStateToLaugh()
    {
        for (var i = 0; i < _componnentsToRemove.Count; i++)
        {
            var monoBehaviour = _componnentsToRemove[i];
            Destroy(monoBehaviour);
        }
        //todo - AnimationThings...
    }
}
