using System;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private readonly int _happinessFloat = Animator.StringToHash("happiness");
    private readonly int _isLeftBool = Animator.StringToHash("isLeft");

    private GameObject PlayerGameObject => transform.parent.gameObject;
    
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        Messenger.Default.Subscribe<HappinessChangedEvent>(OnHappinessChanged);
        Messenger.Default.Subscribe<PlayerChangedDirectionEvent>(OnPlayerChangeDirection);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<HappinessChangedEvent>(OnHappinessChanged);
        Messenger.Default.Unsubscribe<PlayerChangedDirectionEvent>(OnPlayerChangeDirection);
    }

    private void OnPlayerChangeDirection(PlayerChangedDirectionEvent obj)
    {
        SetIsLeft(obj.Direction <= 0f);
    }

    private void OnHappinessChanged(HappinessChangedEvent obj)
    {
        if (obj.GameObject != PlayerGameObject)
        {
            return;
        }
        SetHappiness(obj.Level);
    }


    public void SetIsLeft(bool isLeft)
    {
        animator.SetBool(_isLeftBool, isLeft);
    }

    public void SetHappiness(float happinessNormalized)
    {
        animator.SetFloat(_happinessFloat, happinessNormalized);
    }
}
