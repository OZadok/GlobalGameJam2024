using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private readonly int _happinessFloat = Animator.StringToHash("happiness");
    private readonly int _isLeftBool = Animator.StringToHash("isLeft");
    
    
    
    [SerializeField] private Animator animator;

    
    
    public void SetIsLeft(bool isLeft)
    {
        animator.SetBool(_isLeftBool, isLeft);
    }

    public void SetHappiness(float happinessNormalized)
    {
        animator.SetFloat(_happinessFloat, happinessNormalized);
    }
}
